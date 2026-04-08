using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace Photon.Core.Generator
{
    [Generator]
    public class ShaderUniformAccessorGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // 查找继承 ShaderBase 的 partial 类
            IncrementalValuesProvider<GeneratorSyntaxContext> candidateClasses = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (SyntaxNode node, CancellationToken _) =>
                    node is ClassDeclarationSyntax cds
                    && cds.Modifiers.Any(m => m.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.PartialKeyword)),
                transform: static (GeneratorSyntaxContext ctx, CancellationToken _) => ctx
            );

            IncrementalValueProvider<(Compilation Compilation, ImmutableArray<GeneratorSyntaxContext> Contexts)> compilationAndContexts =
                context.CompilationProvider.Combine(candidateClasses.Collect());

            context.RegisterSourceOutput(compilationAndContexts, Execute);
        }

        private static void Execute(SourceProductionContext spc, (Compilation Compilation, ImmutableArray<GeneratorSyntaxContext> Contexts) source)
        {
            Compilation compilation = source.Compilation;
            ImmutableArray<GeneratorSyntaxContext> contexts = source.Contexts;

            INamedTypeSymbol shaderBaseType = compilation.GetTypeByMetadataName("Photon.Core.Shader.ShaderBase");
            INamedTypeSymbol materialBaseType = compilation.GetTypeByMetadataName("Photon.Core.Material.MaterialBase");
            INamedTypeSymbol uniformEnumType = compilation.GetTypeByMetadataName("Photon.Core.Shader.BuildinShaderUniformType");
            INamedTypeSymbol matPropAttrType = compilation.GetTypeByMetadataName("Photon.Core.Material.MaterialPropertyAttribute");
            INamedTypeSymbol shaderBindingAttrType = compilation.GetTypeByMetadataName("Photon.Core.Shader.Generator.ShaderBindingAttribute");
            INamedTypeSymbol vertexInputInterfaceType = compilation.GetTypeByMetadataName("Photon.Core.Shader.IVertexInput");
            INamedTypeSymbol vertexToFragmentInterfaceType = compilation.GetTypeByMetadataName("Photon.Core.Shader.IVertexToFragment");

            if (shaderBaseType == null || materialBaseType == null)
            {
                return;
            }

            foreach (GeneratorSyntaxContext ctx in contexts)
            {
                if (ctx.Node is not ClassDeclarationSyntax classDecl)
                {
                    continue;
                }

                INamedTypeSymbol classSymbol = ctx.SemanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
                if (classSymbol == null || !InheritsFrom(classSymbol, shaderBaseType))
                {
                    continue;
                }

                // 找到构造函数中的 Material 类型
                INamedTypeSymbol materialType = GetMaterialType(classSymbol, materialBaseType);
                if (materialType == null)
                {
                    continue;
                }

                INamedTypeSymbol vertexInputStruct = FindShaderBindingStruct(classSymbol, shaderBindingAttrType, vertexInputInterfaceType);
                INamedTypeSymbol vertexToFragmentStruct = FindShaderBindingStruct(classSymbol, shaderBindingAttrType, vertexToFragmentInterfaceType);

                // 生成代码
                string code = GenerateCode(classSymbol, materialType, uniformEnumType, matPropAttrType, vertexInputStruct, vertexToFragmentStruct);
                spc.AddSource($"{classSymbol.ContainingNamespace}.{classSymbol.Name}.g.cs", SourceText.From(code, Encoding.UTF8));
            }
        }

        private static bool InheritsFrom(INamedTypeSymbol type, INamedTypeSymbol baseType)
        {
            INamedTypeSymbol current = type.BaseType;
            while (current != null)
            {
                if (SymbolEqualityComparer.Default.Equals(current, baseType))
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        private static INamedTypeSymbol GetMaterialType(INamedTypeSymbol shaderClass, INamedTypeSymbol materialBase)
        {
            foreach (IMethodSymbol ctor in shaderClass.Constructors)
            {
                foreach (IParameterSymbol param in ctor.Parameters)
                {
                    if (param.Type is INamedTypeSymbol nt && InheritsFrom(nt, materialBase))
                    {
                        return nt;
                    }
                }
            }

            return null;
        }

        private static INamedTypeSymbol FindShaderBindingStruct(INamedTypeSymbol shaderClass, INamedTypeSymbol shaderBindingAttrType, INamedTypeSymbol interfaceType)
        {
            if (interfaceType == null)
            {
                return null;
            }

            foreach (INamedTypeSymbol nested in shaderClass.GetTypeMembers())
            {
                if (nested.TypeKind != TypeKind.Struct)
                {
                    continue;
                }

                bool hasBindingAttribute;
                if (shaderBindingAttrType != null)
                {
                    hasBindingAttribute = nested.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, shaderBindingAttrType));
                }
                else
                {
                    hasBindingAttribute = nested.GetAttributes().Any(a => a.AttributeClass?.Name is "ShaderBindingAttribute" or "ShaderBinding");
                }

                if (!hasBindingAttribute)
                {
                    continue;
                }

                if (nested.AllInterfaces.Any(i => SymbolEqualityComparer.Default.Equals(i, interfaceType)))
                {
                    return nested;
                }
            }

            return null;
        }

        private static string GenerateCode(INamedTypeSymbol shaderClass, INamedTypeSymbol materialClass, INamedTypeSymbol uniformEnumType, INamedTypeSymbol matPropAttrType, INamedTypeSymbol vertexInputStruct, INamedTypeSymbol vertexToFragmentStruct)
        {
            string ns = shaderClass.ContainingNamespace.ToDisplayString();
            string shaderName = shaderClass.Name;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// <auto-generated/>");
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using Photon.Core.Geometry;");
            sb.AppendLine("using Photon.Core.Geometry.Fragment;");
            sb.AppendLine("using Photon.Core.Geometry.Vertex;");
            sb.AppendLine("using Photon.Core.Material;");
            sb.AppendLine("using Photon.Core.Shader;");
            sb.AppendLine("using Photon.Math.Vector;");
            sb.AppendLine();
            sb.AppendLine($"namespace {ns}");
            sb.AppendLine("{");
            sb.AppendLine($"    public partial class {shaderName}");
            sb.AppendLine("    {");

            int index = 0;

            if (uniformEnumType != null)
            {
                List<IFieldSymbol> builtIns = uniformEnumType.GetMembers().OfType<IFieldSymbol>().Where(f => f.Name != "Count").ToList();
                foreach (IFieldSymbol field in builtIns)
                {
                    sb.AppendLine($@"        public ShaderUniform u_{field.Name} => material.shaderUniforms[{index}];");
                    index++;
                }
            }

            List<IPropertySymbol> props = materialClass.GetMembers().OfType<IPropertySymbol>()
                .Where(p => matPropAttrType == null || p.GetAttributes().Any(a => SymbolEqualityComparer.Default.Equals(a.AttributeClass, matPropAttrType)))
                .ToList();

            foreach (IPropertySymbol prop in props)
            {
                sb.AppendLine($@"        public ShaderUniform u_{prop.Name} => material.shaderUniforms[{index}];");
                index++;
            }

            sb.AppendLine();
            GenerateBindVertexInput(sb, vertexInputStruct);
            sb.AppendLine();
            GenerateBindVertexToFragment(sb, vertexToFragmentStruct);
            sb.AppendLine();
            GenerateBindFragmentInput(sb, vertexToFragmentStruct);
            sb.AppendLine();
            GenerateHelpers(sb);
            sb.AppendLine("    }");
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static void GenerateBindVertexInput(StringBuilder sb, INamedTypeSymbol vertexInputStruct)
        {
            if (vertexInputStruct == null)
            {
                sb.AppendLine("        public override void BindVertexInput(GeometryObject geometryObject, int vertexIndex, out IVertexInput input)");
                sb.AppendLine("        {");
                sb.AppendLine("            throw new InvalidOperationException(\"未找到实现 IVertexInput 且带有 ShaderBinding 的结构体\");");
                sb.AppendLine("        }");
                return;
            }

            sb.AppendLine("        public override void BindVertexInput(GeometryObject geometryObject, int vertexIndex, out IVertexInput input)");
            sb.AppendLine("        {");
            sb.AppendLine("            ArgumentNullException.ThrowIfNull(geometryObject);");
            sb.AppendLine("            if (vertexIndex < 0 || vertexIndex >= geometryObject.primitive.vertices.Length)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new ArgumentOutOfRangeException(nameof(vertexIndex));");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            Vertex vertex = geometryObject.primitive.vertices[vertexIndex];");
            sb.AppendLine($"            {vertexInputStruct.Name} value = new {vertexInputStruct.Name}();");

            foreach (IFieldSymbol field in vertexInputStruct.GetMembers().OfType<IFieldSymbol>().Where(f => !f.IsStatic))
            {
                string fieldType = field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                string assignment = BuildVertexInputAssignment(field.Name, fieldType);
                sb.AppendLine($"            value.{field.Name} = {assignment};");
            }

            sb.AppendLine("            input = value;");
            sb.AppendLine("        }");
        }

        private static void GenerateBindFragmentInput(StringBuilder sb, INamedTypeSymbol vertexToFragmentStruct)
        {
            if (vertexToFragmentStruct == null)
            {
                sb.AppendLine("        public override void BindFragmentInput(Fragment fragment, out IVertexToFragment input)");
                sb.AppendLine("        {");
                sb.AppendLine("            throw new InvalidOperationException(\"未找到实现 IVertexToFragment 且带有 ShaderBinding 的结构体\");");
                sb.AppendLine("        }");
                return;
            }

            sb.AppendLine("        public override void BindFragmentInput(Fragment fragment, out IVertexToFragment input)");
            sb.AppendLine("        {");
            sb.AppendLine($"            {vertexToFragmentStruct.Name} value = new {vertexToFragmentStruct.Name}();");

            foreach (IFieldSymbol field in vertexToFragmentStruct.GetMembers().OfType<IFieldSymbol>().Where(f => !f.IsStatic))
            {
                string assignment = BuildFragmentInputAssignment(field.Name, field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                sb.AppendLine($"            value.{field.Name} = {assignment};");
            }

            sb.AppendLine("            input = value;");
            sb.AppendLine("        }");
        }

        private static string BuildVertexInputAssignment(string fieldName, string fullyQualifiedTypeName)
        {
            string normalizedName = fieldName.ToLowerInvariant();
            bool isVector2 = fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector2";
            bool isVector3 = fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector3";

            if (isVector3 && normalizedName.Contains("position"))
            {
                return "vertex.position";
            }

            if (isVector2 && (normalizedName.Contains("uv") || normalizedName.Contains("texcoord")))
            {
                return "vertex.uv";
            }

            if (isVector3 && normalizedName.Contains("normal"))
            {
                return "vertex.normal";
            }

            if (fullyQualifiedTypeName == "global::System.Single")
            {
                return $"ReadFloatProperty(geometryObject, vertexIndex, \"{fieldName}\")";
            }

            if (isVector2)
            {
                return $"ReadVector2Property(geometryObject, vertexIndex, \"{fieldName}\")";
            }

            if (isVector3)
            {
                return $"ReadVector3Property(geometryObject, vertexIndex, \"{fieldName}\")";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector4")
            {
                return $"ReadVector4Property(geometryObject, vertexIndex, \"{fieldName}\")";
            }

            return $"throw new InvalidOperationException(\"不支持的 VertexInput 字段类型: {fullyQualifiedTypeName}\")";
        }

        private static void GenerateBindVertexToFragment(StringBuilder sb, INamedTypeSymbol vertexToFragmentStruct)
        {
            if (vertexToFragmentStruct == null)
            {
                sb.AppendLine("        public override void BindVertexToFragment(GeometryObject geometryObject, int vertexIndex, IVertexToFragment output)");
                sb.AppendLine("        {");
                sb.AppendLine("            throw new InvalidOperationException(\"未找到实现 IVertexToFragment 且带有 ShaderBinding 的结构体\");");
                sb.AppendLine("        }");
                return;
            }

            sb.AppendLine("        public override void BindVertexToFragment(GeometryObject geometryObject, int vertexIndex, IVertexToFragment output)");
            sb.AppendLine("        {");
            sb.AppendLine("            ArgumentNullException.ThrowIfNull(geometryObject);");
            sb.AppendLine("            ArgumentNullException.ThrowIfNull(output);");
            sb.AppendLine("            if (vertexIndex < 0 || vertexIndex >= geometryObject.primitive.vertices.Length)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new ArgumentOutOfRangeException(nameof(vertexIndex));");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine($"            if (output is not {vertexToFragmentStruct.Name} value)");
            sb.AppendLine("            {");
            sb.AppendLine("                throw new InvalidOperationException(\"VertexShader 输出类型与 ShaderBinding 的 IVertexToFragment 结构体不匹配\");");
            sb.AppendLine("            }");
            sb.AppendLine();

            foreach (IFieldSymbol field in vertexToFragmentStruct.GetMembers().OfType<IFieldSymbol>().Where(f => !f.IsStatic))
            {
                string writeExpression = BuildVertexOutputWriteExpression(field.Name, field.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                sb.AppendLine($"            int index_{field.Name} = EnsureGeometryPropertySlot(geometryObject, \"{field.Name}\");");
                sb.AppendLine($"            geometryObject.properties[index_{field.Name}][vertexIndex] = {writeExpression};");
            }

            sb.AppendLine("        }");
        }

        private static string BuildVertexOutputWriteExpression(string fieldName, string fullyQualifiedTypeName)
        {
            if (fullyQualifiedTypeName == "global::System.Single")
            {
                return $"new GeometryProperty(value.{fieldName})";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector2")
            {
                return $"new GeometryProperty(value.{fieldName})";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector3")
            {
                return $"new GeometryProperty(value.{fieldName})";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector4")
            {
                return $"new GeometryProperty(value.{fieldName})";
            }

            return $"throw new InvalidOperationException(\"不支持的 VertexToFragment 字段类型: {fullyQualifiedTypeName}\")";
        }

        private static string BuildFragmentInputAssignment(string fieldName, string fullyQualifiedTypeName)
        {
            if (fullyQualifiedTypeName == "global::System.Single")
            {
                return $"ReadFloatProperty(fragment, \"{fieldName}\")";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector2")
            {
                return $"ReadVector2Property(fragment, \"{fieldName}\")";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector3")
            {
                return $"ReadVector3Property(fragment, \"{fieldName}\")";
            }

            if (fullyQualifiedTypeName == "global::Photon.Math.Vector.Vector4")
            {
                return $"ReadVector4Property(fragment, \"{fieldName}\")";
            }

            return $"throw new InvalidOperationException(\"不支持的 IVertexToFragment 字段类型: {fullyQualifiedTypeName}\")";
        }

        private static void GenerateHelpers(StringBuilder sb)
        {
            sb.AppendLine("        private static int EnsureGeometryPropertySlot(GeometryObject geometryObject, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (geometryObject.propertyIndexMap.TryGetValue(propertyName, out int index))");
            sb.AppendLine("            {");
            sb.AppendLine("                EnsurePropertyBuffer(geometryObject, index);");
            sb.AppendLine("                return index;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            index = geometryObject.propertyIndexMap.Count;");
            sb.AppendLine("            geometryObject.propertyIndexMap[propertyName] = index;");
            sb.AppendLine("            EnsurePropertyBuffer(geometryObject, index);");
            sb.AppendLine("            return index;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static void EnsurePropertyBuffer(GeometryObject geometryObject, int index)");
            sb.AppendLine("        {");
            sb.AppendLine("            GeometryProperty[][] properties = geometryObject.properties;");
            sb.AppendLine("            if (properties.Length <= index)");
            sb.AppendLine("            {");
            sb.AppendLine("                Array.Resize(ref properties, index + 1);");
            sb.AppendLine("                geometryObject.properties = properties;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            GeometryProperty[]? buffer = geometryObject.properties[index];");
            sb.AppendLine("            if (buffer == null || buffer.Length != geometryObject.primitive.vertices.Length)");
            sb.AppendLine("            {");
            sb.AppendLine("                geometryObject.properties[index] = new GeometryProperty[geometryObject.primitive.vertices.Length];");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static int GetGeometryPropertyIndex(Dictionary<string, int> propertyIndexMap, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (propertyIndexMap.TryGetValue(propertyName, out int index))");
            sb.AppendLine("            {");
            sb.AppendLine("                return index;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            foreach (KeyValuePair<string, int> pair in propertyIndexMap)");
            sb.AppendLine("            {");
            sb.AppendLine("                if (string.Equals(pair.Key, propertyName, StringComparison.OrdinalIgnoreCase))");
            sb.AppendLine("                {");
            sb.AppendLine("                    return pair.Value;");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            throw new InvalidOperationException($\"几何属性 {propertyName} 不存在，无法绑定 VertexInput\");");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static float ReadFloatProperty(GeometryObject geometryObject, int vertexIndex, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(geometryObject.propertyIndexMap, propertyName);");
            sb.AppendLine("            return geometryObject.properties[index][vertexIndex].floatValue;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector2 ReadVector2Property(GeometryObject geometryObject, int vertexIndex, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(geometryObject.propertyIndexMap, propertyName);");
            sb.AppendLine("            return geometryObject.properties[index][vertexIndex].vector2Value;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector3 ReadVector3Property(GeometryObject geometryObject, int vertexIndex, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(geometryObject.propertyIndexMap, propertyName);");
            sb.AppendLine("            return geometryObject.properties[index][vertexIndex].vector3Value;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector4 ReadVector4Property(GeometryObject geometryObject, int vertexIndex, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(geometryObject.propertyIndexMap, propertyName);");
            sb.AppendLine("            return geometryObject.properties[index][vertexIndex].vector4Value;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static int GetGeometryPropertyIndex(Fragment fragment, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (fragment.propertyIndexMap.TryGetValue(propertyName, out int index))");
            sb.AppendLine("            {");
            sb.AppendLine("                return index;");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            foreach (KeyValuePair<string, int> pair in fragment.propertyIndexMap)");
            sb.AppendLine("            {");
            sb.AppendLine("                if (string.Equals(pair.Key, propertyName, StringComparison.OrdinalIgnoreCase))");
            sb.AppendLine("                {");
            sb.AppendLine("                    return pair.Value;");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            throw new InvalidOperationException($\"几何属性 {propertyName} 不存在，无法绑定 Fragment 输入\");");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static float ReadFloatProperty(Fragment fragment, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(fragment, propertyName);");
            sb.AppendLine("            return fragment.properties[index].floatValue;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector2 ReadVector2Property(Fragment fragment, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(fragment, propertyName);");
            sb.AppendLine("            return fragment.properties[index].vector2Value;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector3 ReadVector3Property(Fragment fragment, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(fragment, propertyName);");
            sb.AppendLine("            return fragment.properties[index].vector3Value;");
            sb.AppendLine("        }");
            sb.AppendLine();

            sb.AppendLine("        private static Vector4 ReadVector4Property(Fragment fragment, string propertyName)");
            sb.AppendLine("        {");
            sb.AppendLine("            int index = GetGeometryPropertyIndex(fragment, propertyName);");
            sb.AppendLine("            return fragment.properties[index].vector4Value;");
            sb.AppendLine("        }");
        }
    }
}