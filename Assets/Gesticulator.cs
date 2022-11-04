using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Python.Runtime;

using UnityEngine;

public class Gesticulator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RunGesticulator();  
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public static void AddEnvPath(params string[] paths)
        {      // PC에 설정되어 있는 환경 변수를 가져온다.
            var envPaths = Environment.GetEnvironmentVariable("PATH").Split(Path.PathSeparator).ToList();
            // 중복 환경 변수가 없으면 list에 넣는다.
            envPaths.InsertRange(  0,   paths.Where( x => x.Length > 0 && !envPaths.Contains(x) ).ToArray()  );
            // 환경 변수를 다시 설정한다.
            Environment.SetEnvironmentVariable("PATH", string.Join(Path.PathSeparator.ToString(), envPaths), EnvironmentVariableTarget.Process);
        }


    static void RunGesticulator()

        {
            Runtime.PythonDLL = @"C:\Users\CY\AppData\Local\Programs\Python\Python38\python38.dll";

            // 아까 where.exe python으로 나온 anaconda 설치 경로를 설정

            var PYTHON_HOME = Environment.ExpandEnvironmentVariables(@"C:\Users\CY\AppData\Local\Programs\Python\Python38");
            // 환경 변수 설정
            AddEnvPath(PYTHON_HOME, Path.Combine(PYTHON_HOME, @"Library\bin")); 
            // Python 홈 설정.
            PythonEngine.PythonHome = PYTHON_HOME;
            // 모듈 패키지 패스 설정.
            PythonEngine.PythonPath = string.Join(

                Path.PathSeparator.ToString(),
                new string[] {
                  PythonEngine.PythonPath,
                    // pip하면 설치되는 패키지 폴더.
                     Path.Combine(PYTHON_HOME, @"Lib\site-packages"),  
                    // 개인 패키지 폴더
                    "C:\\Users\\CY\\Desktop\\pythonRuntime\\Assets\\pythonCode\\gesticulator"
                }
            );
            // Python 엔진 초기화
            PythonEngine.Initialize();     
            // Global Interpreter Lock을 취득
            using (Py.GIL())
            {

               dynamic demoTest = Py.Import("demo.demoUnity");
               dynamic result = demoTest.main("C:\\Users\\CY\\Desktop\\pythonRuntime\\Assets\\pythonCode\\gesticulator\\demo\\input\\jeremy_howard.wav", "C:\\Users\\CY\\Desktop\\pythonRuntime\\Assets\\pythonCode\\gesticulator\\demo\\input\\jeremy_howard.txt");
            Debug.Log(result);
            Debug.Log(result.Length());
            }    
            // python 환경을 종료한다.
            PythonEngine.Shutdown();
            

        }   //   static void Main(string[] args)

    
}
