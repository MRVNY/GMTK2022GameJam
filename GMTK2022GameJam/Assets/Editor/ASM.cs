using UnityEditor;
using UnityEngine;
 
[InitializeOnLoad]
public class ASM
{
    static ASM()
    {
        PlayerSettings.WebGL.linkerTarget = WebGLLinkerTarget.Asm;
        PlayerSettings.WebGL.memorySize = 512;
    }
}