using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using ScreenCaptureCore.Util;
using System.Linq;

namespace SaveFileTest
{
    [TestClass]
    public class UnitTest1
    {
        // Reflection으로 가져올 타입 설정
        private static BindingFlags ALLBIND = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        // 라이브러리 및 실행 파일을 동적으로 읽어 올 클래스
        private readonly Assembly ASSEMBLY;
        // 클래스 typeof
        private readonly Type PROGRAM_TYPE;
        // 메소드 종류
        private readonly MethodInfo[] PROGRAM_METHODS;
        // 맴버 필드 종류
        private readonly FieldInfo[] PROGRAM_FIELDS;

        public UnitTest1()
        {
            // 라이브러리 및 실행 파일을 읽어 온다.
            ASSEMBLY = Assembly.LoadFrom("Example.exe");
            // 해당 파일에서 Program 클래스를 읽어 온다.
            PROGRAM_TYPE = ASSEMBLY.GetType("Example.Program");
            // 메소드를 읽어 온다.
            PROGRAM_METHODS = PROGRAM_TYPE.GetMethods(ALLBIND);
            // 맴버 필드를 읽어 온다.
            PROGRAM_FIELDS = PROGRAM_TYPE.GetFields(ALLBIND);
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
