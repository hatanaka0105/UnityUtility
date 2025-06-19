using System.Reflection;
using System.Reflection.Emit;
using System.Collections.Generic;

namespace UnityCustomExtension
{
    public static class EnumBuildFromString
    {
        public static System.Type BuildEnum(List<string> strings)
        {
            AssemblyName asmName = new AssemblyName { Name = "MyAssembly" };
            System.AppDomain domain = System.AppDomain.CurrentDomain;

            AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule("MyModule");
            EnumBuilder enumBuilder = moduleBuilder.DefineEnum("MyNamespace.MyEnum", TypeAttributes.Public, typeof(int));

            for (int i = 0; i < strings.Count; ++i)
            {
                enumBuilder.DefineLiteral(strings[i], i + 1);
            }

            var info = enumBuilder.CreateTypeInfo();
            return info.AsType();
        }
    }
}