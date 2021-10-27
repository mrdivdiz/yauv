using System;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class Util
{
	// Token: 0x060004E7 RID: 1255 RVA: 0x000206D0 File Offset: 0x0001E8D0
	public static bool IsSaneNumber(float f)
	{
		return !float.IsNaN(f) && f != float.PositiveInfinity && f != float.NegativeInfinity && f <= 1E+12f && f >= -1E+12f;
	}

	// Token: 0x060004E8 RID: 1256 RVA: 0x00020720 File Offset: 0x0001E920
	public static Vector3 Clamp(Vector3 v, float length)
	{
		float magnitude = v.magnitude;
		if (magnitude > length)
		{
			return v / magnitude * length;
		}
		return v;
	}

	// Token: 0x060004E9 RID: 1257 RVA: 0x0002074C File Offset: 0x0001E94C
	public static float Mod(float x, float period)
	{
		float num = x % period;
		return (num < 0f) ? (num + period) : num;
	}

	// Token: 0x060004EA RID: 1258 RVA: 0x00020774 File Offset: 0x0001E974
	public static int Mod(int x, int period)
	{
		int num = x % period;
		return (num < 0) ? (num + period) : num;
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00020798 File Offset: 0x0001E998
	public static float Mod(float x)
	{
		return Util.Mod(x, 1f);
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x000207A8 File Offset: 0x0001E9A8
	public static int Mod(int x)
	{
		return Util.Mod(x, 1);
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x000207B4 File Offset: 0x0001E9B4
	public static float CyclicDiff(float high, float low, float period, bool skipWrap)
	{
		if (!skipWrap)
		{
			high = Util.Mod(high, period);
			low = Util.Mod(low, period);
		}
		return (high < low) ? (high + period - low) : (high - low);
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x000207F0 File Offset: 0x0001E9F0
	public static int CyclicDiff(int high, int low, int period, bool skipWrap)
	{
		if (!skipWrap)
		{
			high = Util.Mod(high, period);
			low = Util.Mod(low, period);
		}
		return (high < low) ? (high + period - low) : (high - low);
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x0002082C File Offset: 0x0001EA2C
	public static float CyclicDiff(float high, float low, float period)
	{
		return Util.CyclicDiff(high, low, period, false);
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00020838 File Offset: 0x0001EA38
	public static int CyclicDiff(int high, int low, int period)
	{
		return Util.CyclicDiff(high, low, period, false);
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00020844 File Offset: 0x0001EA44
	public static float CyclicDiff(float high, float low)
	{
		return Util.CyclicDiff(high, low, 1f, false);
	}

	// Token: 0x060004F2 RID: 1266 RVA: 0x00020854 File Offset: 0x0001EA54
	public static int CyclicDiff(int high, int low)
	{
		return Util.CyclicDiff(high, low, 1, false);
	}

	// Token: 0x060004F3 RID: 1267 RVA: 0x00020860 File Offset: 0x0001EA60
	public static bool CyclicIsLower(float compared, float comparedTo, float reference, float period)
	{
		compared = Util.Mod(compared, period);
		comparedTo = Util.Mod(comparedTo, period);
		return Util.CyclicDiff(compared, reference, period, true) < Util.CyclicDiff(comparedTo, reference, period, true);
	}

	// Token: 0x060004F4 RID: 1268 RVA: 0x0002089C File Offset: 0x0001EA9C
	public static bool CyclicIsLower(int compared, int comparedTo, int reference, int period)
	{
		compared = Util.Mod(compared, period);
		comparedTo = Util.Mod(comparedTo, period);
		return Util.CyclicDiff(compared, reference, period, true) < Util.CyclicDiff(comparedTo, reference, period, true);
	}

	// Token: 0x060004F5 RID: 1269 RVA: 0x000208D8 File Offset: 0x0001EAD8
	public static bool CyclicIsLower(float compared, float comparedTo, float reference)
	{
		return Util.CyclicIsLower(compared, comparedTo, reference, 1f);
	}

	// Token: 0x060004F6 RID: 1270 RVA: 0x000208E8 File Offset: 0x0001EAE8
	public static bool CyclicIsLower(int compared, int comparedTo, int reference)
	{
		return Util.CyclicIsLower(compared, comparedTo, reference, 1);
	}

	// Token: 0x060004F7 RID: 1271 RVA: 0x000208F4 File Offset: 0x0001EAF4
	public static float CyclicLerp(float a, float b, float t, float period)
	{
		if (Mathf.Abs(b - a) <= period / 2f)
		{
			return a * (1f - t) + b * t;
		}
		if (b < a)
		{
			a -= period;
		}
		else
		{
			b -= period;
		}
		return Util.Mod(a * (1f - t) + b * t);
	}

	// Token: 0x060004F8 RID: 1272 RVA: 0x0002094C File Offset: 0x0001EB4C
	public static Vector3 ProjectOntoPlane(Vector3 v, Vector3 normal)
	{
		return v - Vector3.Project(v, normal);
	}

	// Token: 0x060004F9 RID: 1273 RVA: 0x0002095C File Offset: 0x0001EB5C
	public static Vector3 SetHeight(Vector3 originalVector, Vector3 referenceHeightVector, Vector3 upVector)
	{
		Vector3 a = Util.ProjectOntoPlane(originalVector, upVector);
		Vector3 b = Vector3.Project(referenceHeightVector, upVector);
		return a + b;
	}

	// Token: 0x060004FA RID: 1274 RVA: 0x00020980 File Offset: 0x0001EB80
	public static Vector3 GetHighest(Vector3 a, Vector3 b, Vector3 upVector)
	{
		if (Vector3.Dot(a, upVector) >= Vector3.Dot(b, upVector))
		{
			return a;
		}
		return b;
	}

	// Token: 0x060004FB RID: 1275 RVA: 0x00020998 File Offset: 0x0001EB98
	public static Vector3 GetLowest(Vector3 a, Vector3 b, Vector3 upVector)
	{
		if (Vector3.Dot(a, upVector) <= Vector3.Dot(b, upVector))
		{
			return a;
		}
		return b;
	}

	// Token: 0x060004FC RID: 1276 RVA: 0x000209B0 File Offset: 0x0001EBB0
	public static Matrix4x4 RelativeMatrix(Transform t, Transform relativeTo)
	{
		return relativeTo.worldToLocalMatrix * t.localToWorldMatrix;
	}

	// Token: 0x060004FD RID: 1277 RVA: 0x000209C4 File Offset: 0x0001EBC4
	public static Vector3 TransformVector(Matrix4x4 m, Vector3 v)
	{
		return m.MultiplyPoint(v) - m.MultiplyPoint(Vector3.zero);
	}

	// Token: 0x060004FE RID: 1278 RVA: 0x000209E0 File Offset: 0x0001EBE0
	public static Vector3 TransformVector(Transform t, Vector3 v)
	{
		return Util.TransformVector(t.localToWorldMatrix, v);
	}

	// Token: 0x060004FF RID: 1279 RVA: 0x000209F0 File Offset: 0x0001EBF0
	public static void TransformFromMatrix(Matrix4x4 matrix, Transform trans)
	{
		trans.rotation = Util.QuaternionFromMatrix(matrix);
		trans.position = matrix.GetColumn(3);
	}

	// Token: 0x06000500 RID: 1280 RVA: 0x00020A1C File Offset: 0x0001EC1C
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
	{
		Quaternion result = default(Quaternion);
		result.w = Mathf.Sqrt(Mathf.Max(0f, 1f + m[0, 0] + m[1, 1] + m[2, 2])) / 2f;
		result.x = Mathf.Sqrt(Mathf.Max(0f, 1f + m[0, 0] - m[1, 1] - m[2, 2])) / 2f;
		result.y = Mathf.Sqrt(Mathf.Max(0f, 1f - m[0, 0] + m[1, 1] - m[2, 2])) / 2f;
		result.z = Mathf.Sqrt(Mathf.Max(0f, 1f - m[0, 0] - m[1, 1] + m[2, 2])) / 2f;
		result.x *= Mathf.Sign(result.x * (m[2, 1] - m[1, 2]));
		result.y *= Mathf.Sign(result.y * (m[0, 2] - m[2, 0]));
		result.z *= Mathf.Sign(result.z * (m[1, 0] - m[0, 1]));
		return result;
	}

	// Token: 0x06000501 RID: 1281 RVA: 0x00020BB8 File Offset: 0x0001EDB8
	public static Matrix4x4 MatrixFromQuaternion(Quaternion q)
	{
		return Util.CreateMatrix(q * Vector3.right, q * Vector3.up, q * Vector3.forward, Vector3.zero);
	}

	// Token: 0x06000502 RID: 1282 RVA: 0x00020BF0 File Offset: 0x0001EDF0
	public static Matrix4x4 MatrixFromQuaternionPosition(Quaternion q, Vector3 p)
	{
		Matrix4x4 result = Util.MatrixFromQuaternion(q);
		result.SetColumn(3, p);
		result[3, 3] = 1f;
		return result;
	}

	// Token: 0x06000503 RID: 1283 RVA: 0x00020C24 File Offset: 0x0001EE24
	public static Matrix4x4 MatrixSlerp(Matrix4x4 a, Matrix4x4 b, float t)
	{
		t = Mathf.Clamp01(t);
		Matrix4x4 result = Util.MatrixFromQuaternion(Quaternion.Slerp(Util.QuaternionFromMatrix(a), Util.QuaternionFromMatrix(b), t));
		result.SetColumn(3, a.GetColumn(3) * (1f - t) + b.GetColumn(3) * t);
		result[3, 3] = 1f;
		return result;
	}

	// Token: 0x06000504 RID: 1284 RVA: 0x00020C90 File Offset: 0x0001EE90
	public static Matrix4x4 CreateMatrix(Vector3 right, Vector3 up, Vector3 forward, Vector3 position)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(0, right);
		identity.SetColumn(1, up);
		identity.SetColumn(2, forward);
		identity.SetColumn(3, position);
		identity[3, 3] = 1f;
		return identity;
	}

	// Token: 0x06000505 RID: 1285 RVA: 0x00020CEC File Offset: 0x0001EEEC
	public static Matrix4x4 CreateMatrixPosition(Vector3 position)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		identity.SetColumn(3, position);
		identity[3, 3] = 1f;
		return identity;
	}

	// Token: 0x06000506 RID: 1286 RVA: 0x00020D1C File Offset: 0x0001EF1C
	public static void TranslateMatrix(ref Matrix4x4 m, Vector3 position)
	{
		m.SetColumn(3, (Vector4)m.GetColumn(3) + (Vector4)position);
		m[3, 3] = 1f;
	}

	// Token: 0x06000507 RID: 1287 RVA: 0x00020D54 File Offset: 0x0001EF54
	public static Vector3 ConstantSlerp(Vector3 from, Vector3 to, float angle)
	{
		float t = Mathf.Min(1f, angle / Vector3.Angle(from, to));
		return Vector3.Slerp(from, to, t);
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x00020D80 File Offset: 0x0001EF80
	public static Quaternion ConstantSlerp(Quaternion from, Quaternion to, float angle)
	{
		float t = Mathf.Min(1f, angle / Quaternion.Angle(from, to));
		return Quaternion.Slerp(from, to, t);
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x00020DAC File Offset: 0x0001EFAC
	public static Vector3 ConstantLerp(Vector3 from, Vector3 to, float length)
	{
		return from + Util.Clamp(to - from, length);
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x00020DC4 File Offset: 0x0001EFC4
	public static Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
	{
		Vector3 from = Vector3.Lerp(a, b, t);
		Vector3 vector = Vector3.Lerp(b, c, t);
		Vector3 to = Vector3.Lerp(c, d, t);
		Vector3 from2 = Vector3.Lerp(from, vector, t);
		Vector3 to2 = Vector3.Lerp(vector, to, t);
		return Vector3.Lerp(from2, to2, t);
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x00020E10 File Offset: 0x0001F010
	public static GameObject Create3dText(Font font, string text, Vector3 position, float size, Color color)
	{
		GameObject gameObject = new GameObject("text_" + text);
		TextMesh textMesh = gameObject.AddComponent(typeof(TextMesh)) as TextMesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		textMesh.font = font;
		gameObject.GetComponent<Renderer>().material = font.material;
		gameObject.GetComponent<Renderer>().material.color = color;
		textMesh.text = text;
		gameObject.transform.localScale = Vector3.one * size;
		gameObject.transform.Translate(position);
		return gameObject;
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x00020EAC File Offset: 0x0001F0AC
	public static float[] GetLineSphereIntersections(Vector3 lineStart, Vector3 lineDir, Vector3 sphereCenter, float sphereRadius)
	{
		float sqrMagnitude = lineDir.sqrMagnitude;
		float num = 2f * (Vector3.Dot(lineStart, lineDir) - Vector3.Dot(lineDir, sphereCenter));
		float num2 = lineStart.sqrMagnitude + sphereCenter.sqrMagnitude - 2f * Vector3.Dot(lineStart, sphereCenter) - sphereRadius * sphereRadius;
		float num3 = num * num - 4f * sqrMagnitude * num2;
		if (num3 < 0f)
		{
			return null;
		}
		float num4 = (-num - Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
		float num5 = (-num + Mathf.Sqrt(num3)) / (2f * sqrMagnitude);
		if (num4 < num5)
		{
			return new float[]
			{
				num4,
				num5
			};
		}
		return new float[]
		{
			num5,
			num4
		};
	}
}
