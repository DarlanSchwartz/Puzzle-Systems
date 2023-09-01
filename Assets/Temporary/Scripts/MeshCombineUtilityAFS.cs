// Decompiled with JetBrains decompiler
// Type: MeshCombineUtilityAFS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FA5F6F7C-8BB3-4830-A460-B436C65731C9
// Assembly location: E:\SteamLibrary\steamapps\common\ConSim2015\ConSim2015_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MeshCombineUtilityAFS
{
    public static Mesh Combine(
      MeshCombineUtilityAFS.MeshInstance[] combines,
      bool bakeGroundLightingGrass,
      bool bakeGroundLightingFoliage,
      float randomBrightness,
      float randomPulse,
      float randomBending,
      float randomFluttering,
      Color HealthyColor,
      Color DryColor,
      float NoiseSpread,
      bool bakeScale,
      bool simplyCombine,
      float NoiseSpreadFoliage)
    {
        int length1 = 0;
        int length2 = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
            {
                length1 += combine.mesh.vertexCount;
                length2 += combine.mesh.GetTriangles(combine.subMeshIndex).Length;
            }
        }
        Vector3[] dst1 = new Vector3[length1];
        Vector3[] dst2 = new Vector3[length1];
        Vector4[] dst3 = new Vector4[length1];
        Vector2[] dst4 = new Vector2[length1];
        Vector2[] dst5 = new Vector2[length1];
        Color[] dst6 = new Color[length1];
        int[] numArray = new int[length2];
        int offset = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
                MeshCombineUtilityAFS.Copy(combine.mesh.vertexCount, combine.mesh.vertices, dst1, ref offset, combine.transform);
        }
        offset = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
            {
                Matrix4x4 transform = combine.transform;
                transform = transform.inverse.transpose;
                if (bakeGroundLightingGrass)
                    MeshCombineUtilityAFS.CopyNormalGround(combine.mesh.vertexCount, combine.mesh.normals, dst2, ref offset, transform, combine.groundNormal);
                else
                    MeshCombineUtilityAFS.CopyNormal(combine.mesh.vertexCount, combine.mesh.normals, dst2, ref offset, transform);
            }
        }
        offset = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
            {
                Matrix4x4 transform = combine.transform;
                transform = transform.inverse.transpose;
                MeshCombineUtilityAFS.CopyTangents(combine.mesh.vertexCount, combine.mesh.tangents, dst3, ref offset, transform);
            }
        }
        offset = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
                MeshCombineUtilityAFS.Copy(combine.mesh.vertexCount, combine.mesh.uv, dst4, ref offset);
        }
        offset = 0;
        if (bakeGroundLightingFoliage)
        {
            foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
            {
                if ((bool)((Object)combine.mesh))
                    MeshCombineUtilityAFS.Copy_uv1(combine.mesh.vertexCount, combine.mesh.uv, dst5, ref offset, new Vector2(combine.groundNormal.x, combine.groundNormal.z));
            }
            offset = 0;
        }
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
            {
                if (bakeGroundLightingGrass)
                    MeshCombineUtilityAFS.CopyColors_grass(combine.mesh.vertexCount, combine.mesh.colors, dst6, ref offset, HealthyColor, DryColor, NoiseSpread, combine.pivot);
                else
                    MeshCombineUtilityAFS.CopyColors(combine.mesh.vertexCount, combine.mesh.colors, dst6, ref offset, combine.scale, bakeScale, combine.pivot, NoiseSpreadFoliage, randomPulse, randomFluttering, randomBrightness, randomBending);
            }
        }
        int num1 = 0;
        int num2 = 0;
        foreach (MeshCombineUtilityAFS.MeshInstance combine in combines)
        {
            if ((bool)((Object)combine.mesh))
            {
                int[] triangles = combine.mesh.GetTriangles(combine.subMeshIndex);
                for (int index = 0; index < triangles.Length; ++index)
                    numArray[index + num1] = triangles[index] + num2;
                num1 += triangles.Length;
                num2 += combine.mesh.vertexCount;
            }
        }
        Mesh mesh = new Mesh();
        mesh.name = "Combined Mesh";
        mesh.vertices = dst1;
        mesh.normals = dst2;
        mesh.colors = dst6;
        mesh.uv = dst4;
        if (bakeGroundLightingFoliage)
            mesh.uv = dst5;
        mesh.tangents = dst3;
        mesh.triangles = numArray;
        mesh.Optimize();
        return mesh;
    }

    private static void Copy(
      int vertexcount,
      Vector3[] src,
      Vector3[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
        for (int index = 0; index < src.Length; ++index)
            dst[index + offset] = transform.MultiplyPoint(src[index]);
        offset += vertexcount;
    }

    private static void CopyNormal(
      int vertexcount,
      Vector3[] src,
      Vector3[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
        for (int index = 0; index < src.Length; ++index)
            dst[index + offset] = transform.MultiplyVector(src[index]).normalized;
        offset += vertexcount;
    }

    private static void CopyNormalGround(
      int vertexcount,
      Vector3[] src,
      Vector3[] dst,
      ref int offset,
      Matrix4x4 transform,
      Vector3 groundNormal)
    {
        for (int index = 0; index < src.Length; ++index)
            dst[index + offset] = groundNormal;
        offset += vertexcount;
    }

    private static void Copy(int vertexcount, Vector2[] src, Vector2[] dst, ref int offset)
    {
        for (int index = 0; index < src.Length; ++index)
            dst[index + offset] = src[index];
        offset += vertexcount;
    }

    private static void Copy_uv1(
      int vertexcount,
      Vector2[] src,
      Vector2[] dst,
      ref int offset,
      Vector2 groundNormal)
    {
        for (int index = 0; index < src.Length; ++index)
            dst[index + offset] = groundNormal;
        offset += vertexcount;
    }

    private static void CopyColors(
      int vertexcount,
      Color[] src,
      Color[] dst,
      ref int offset,
      float scale,
      bool bakeScale,
      Vector3 pivot,
      float NoiseSpread,
      float randomPulse,
      float randomFluttering,
      float randomBrightness,
      float randomBending)
    {
        for (int index = 0; index < src.Length; ++index)
        {
            float num = Mathf.PerlinNoise(pivot.x, pivot.y);
            src[index].r += randomPulse * num;
            src[index].g *= (float)(1.0 + (double)randomFluttering * ((double)num - 0.5));
            if (bakeScale)
                src[index].b = (float)((double)src[index].b * (double)scale * (1.0 + (double)randomBending * (double)num));
            else
                src[index].b *= (float)(1.0 + (double)randomBending * (double)num);
            src[index].a -= num * randomBrightness;
            dst[index + offset] = src[index];
        }
        offset += vertexcount;
    }

    private static void CopyColors_groundNormal_old(
      int vertexcount,
      Color[] src,
      Color[] dst,
      ref int offset,
      Color RandColor,
      Vector2 groundNormal)
    {
        for (int index = 0; index < src.Length; ++index)
        {
            dst[index + offset] = src[index] + RandColor;
            dst[index + offset].r = groundNormal.x;
            dst[index + offset].g = groundNormal.y;
        }
        offset += vertexcount;
    }

    private static void CopyColors_grass(
      int vertexcount,
      Color[] src,
      Color[] dst,
      ref int offset,
      Color HealthyColor,
      Color DryColor,
      float NoiseSpread,
      Vector3 pivot)
    {
        Color color = Color.Lerp(HealthyColor, DryColor, Mathf.PerlinNoise(pivot.x * NoiseSpread, pivot.y * NoiseSpread));
        for (int index = 0; index < src.Length; ++index)
        {
            dst[index + offset].a = src[index].a;
            dst[index + offset].r = Mathf.Lerp(1f, color.r, color.a) * src[index].b;
            dst[index + offset].g = Mathf.Lerp(1f, color.g, color.a) * src[index].b;
            dst[index + offset].b = Mathf.Lerp(1f, color.b, color.a) * src[index].b;
        }
        offset += vertexcount;
    }

    private static void CopyTangents(
      int vertexcount,
      Vector4[] src,
      Vector4[] dst,
      ref int offset,
      Matrix4x4 transform)
    {
        for (int index = 0; index < src.Length; ++index)
        {
            Vector4 vector4 = src[index];
            Vector3 v = new Vector3(vector4.x, vector4.y, vector4.z);
            v = transform.MultiplyVector(v).normalized;
            dst[index + offset] = new Vector4(v.x, v.y, v.z, vector4.w);
        }
        offset += vertexcount;
    }

    public struct MeshInstance
    {
        public Mesh mesh;
        public int subMeshIndex;
        public Matrix4x4 transform;
        public Vector3 groundNormal;
        public float scale;
        public Vector3 pivot;
    }
}
