using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GIKCore.Utilities
{
    public class IMath
    {
        /// <summary>
        /// Divide a by b; if b == 0 => b = replaceZero
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="replaceZero">default = 1; it have to # 0</param>
        /// <returns></returns>
        public static float Divide(float a, float b, float replaceZero = 1f)
        {
            if (replaceZero == 0f) replaceZero = 1f;
            if (b == 0) b = replaceZero;
            return a / b;
        }
        public static double Divide2(double a, double b, double replaceZero = 1)
        {
            if (replaceZero == 0f) replaceZero = 1;
            if (b == 0) b = replaceZero;
            return a / b;
        }
        /// <summary> x < minLimit => x = minLimit </summary>
        public static float MinLimit(float x, float minLimit)
        {
            if (x < minLimit) return minLimit;
            return x;
        }
        /// <summary> x > maxLimit => x = maxLimit </summary>
        public static float MaxLimit(float x, float maxLimit)
        {
            if (x > maxLimit) return maxLimit;
            return x;
        }
        public static float MinMaxLimit(float x, float minLimit, float maxLimit)
        {
            float r1 = MinLimit(x, minLimit);
            float r2 = MaxLimit(r1, maxLimit);
            return r2;
        }
        /// <summary> 0 <= x <= 1 </summary>
        public static float LimitAmount(float x)
        {
            return MinMaxLimit(x, 0f, 1f);
        }
        public static string GetString(string s, string defaultValue = "", bool ignoreEmpty = true)
        {
            if (s == null) return defaultValue;
            if (ignoreEmpty && string.IsNullOrEmpty(s)) return defaultValue;
            return s;
        }

        public static int ParseInt(string s, int defaultValue = 0)
        {
            int r = defaultValue;
            int.TryParse(s, out r);
            return r;
        }
        public static long ParseLong(string s, long defaultValue = 0)
        {
            long r = defaultValue;
            long.TryParse(s, out r);
            return r;
        }
        public static float ParseFloat(string s, float defaultValue = 0f)
        {
            float r = defaultValue;
            float.TryParse(s, out r);
            return r;
        }
        public static int RandomInt(int fromInclusive, int toInclusive, int time = 1)
        {
            time = Mathf.Max(time, 1);
            int result = fromInclusive;
            for (int i = 0; i < time; i++)
            {
                float tmp = fromInclusive + (toInclusive - fromInclusive) * Random.value;
                result = System.Convert.ToInt32(tmp);
            }
            return result;
        }
        public static long RandomLong(long fromInclusive, long toInclusive, int time = 1)
        {
            time = Mathf.Max(time, 1);
            long result = fromInclusive;
            for (int i = 0; i < time; i++)
            {
                float tmp = fromInclusive + (toInclusive - fromInclusive) * Random.value;
                result = System.Convert.ToInt64(tmp);
            }
            return result;
        }
        public static float RandomFloat(float fromInclusive, float toInclusive, int time = 1)
        {
            time = Mathf.Max(time, 1);
            float result = fromInclusive;
            for (int i = 0; i < time; i++)
            {
                float rnd = Random.value;
                result = fromInclusive + (toInclusive - fromInclusive) * rnd;
            }
            return result;
        }
        public static float ToDegree(float rad)
        {
            return rad * (180f / Mathf.PI);
        }
        public static float ToRadian(float degree)
        {
            return degree * (Mathf.PI / 180f);
        }
        public static float AngleBetweenVector2(Vector2 v1, Vector2 v2)
        {
            Vector2 dif = v2 - v1;
            float sign = (v2.y < v1.y) ? -1f : 1f;
            return Vector2.Angle(Vector2.right, dif) * sign;
        }
        public static List<int> Shuffle(int fromInclusive, int toExclusive, int time = 5)
        {
            List<int> lst = new List<int>();
            for (int i = fromInclusive; i < toExclusive; i++)
                lst.Add(i);
            int num = lst.Count;
            while (num > 1)
            {
                num--;
                int k = RandomInt(0, num, time);
                int tmp = lst[k];
                lst[k] = lst[num];
                lst[num] = tmp;
            }
            return lst;
        }
        public static List<int> ConvertListLongToListInt(List<long> lstLong)
        {
            List<int> lstInt = new List<int>();
            int numLong = lstLong.Count;
            for (int i = 0; i < numLong; i++)
            {
                int tmp = unchecked((int)lstLong[i]);// It'll throw OverflowException in checked context if the value doesn't fit in an int:
                lstInt.Add(tmp);
            }

            return lstInt;
        }
        public static List<long> ConvertListIntToListLong(List<int> lstInt)
        {
            List<long> lstLong = new List<long>();
            int numLong = lstInt.Count;
            for (int i = 0; i < numLong; i++)
            {
                lstLong.Add(lstInt[i]);
            }

            return lstLong;
        }
        public static List<int> GetDigitsI(string s)
        {
            List<int> ret = new List<int>();
            string[] digits = Regex.Split(s, @"\D+");
            foreach (string v in digits)
            {
                int num;
                if (int.TryParse(v, out num))
                    ret.Add(num);
            }
            return ret;
        }
        public static List<long> GetDigitsL(string s)
        {
            List<long> ret = new List<long>();
            string[] digits = Regex.Split(s, @"\D+");
            foreach (string v in digits)
            {
                long num;
                if (long.TryParse(v, out num))
                    ret.Add(num);
            }
            return ret;
        }
        public static List<float> GetDigitF(string s)
        {
            List<float> ret = new List<float>();
            string[] digits = Regex.Split(s, @"\D+");
            foreach (string v in digits)
            {
                float num;
                if (float.TryParse(v, out num))
                    ret.Add(num);
            }
            return ret;
        }

        public static string LongToRoman(long num)
        {
            string[] romanLetters = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
            long[] numbers = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

            string romanResult = "";
            int index = 0;
            while (num != 0)
            {
                if (num >= numbers[index])
                {
                    num -= numbers[index];
                    romanResult += romanLetters[index];
                }
                else index++;
            }
            return romanResult;
        }

        /// <summary> Lay toan bo so to hop chap có k phan tu caa mot list </summary>
        public static List<List<T>> GetCombinationOfAList<T>(int k, List<T> lst)
        {
            int n = (lst == null) ? 0 : lst.Count;
            List<List<T>> result = new List<List<T>>();
            if (n > 0 && n >= k)
            {
                int[] tmp = new int[n + 1];
                ICallback.CallFunc print = () =>
                {
                    List<T> combination = new List<T>();
                    for (int i = 1; i <= k; i++)
                    {
                        int index = tmp[i] - 1;
                        combination.Add(lst[index]);
                    }
                    if (combination.Count == k)
                        result.Add(combination);
                };

                //init            
                for (int i = 1; i <= k; i++)
                    tmp[i] = i;
                int idx = 0;
                do
                {
                    print();
                    idx = k;
                    while (idx > 0 && tmp[idx] == (n - k + idx)) --idx;
                    if (idx > 0)
                    {
                        tmp[idx]++;
                        for (int i = idx + 1; i <= k; i++)
                        {
                            tmp[i] = tmp[i - 1] + 1;
                        }
                    }
                } while (idx != 0);
            }

            return result;
        }
        public static bool CheckPercentRate(int rate)
        {
            rate = (int)MinMaxLimit(rate, 0, 100);
            if (rate == 0) return false;
            if (rate == 100) return true;

            List<int> sample = new List<int>();
            int sampleCount = 100;

            for (int i = 0; i < rate; i++) sample.Add(1);
            for (int i = rate; i < sampleCount; i++) sample.Add(0);

            int idx = (int)MinMaxLimit(Mathf.Floor(Random.value * sampleCount), 0, sampleCount - 1);
            return sample[idx] == 1;
        }
        public static string GenRandomUUID()
        {
            System.Guid uuid = System.Guid.NewGuid();
            return uuid.ToString();
        }
        /// <summary> Chuyen dong thang, bien doi deu </summary>
        public static float GetMoveTime(List<Vector3> lstPoints, float velocity, float defaultValue)
        {
            defaultValue = (defaultValue < 0f) ? 0f : defaultValue;

            float s = 0;
            for (int i = 0; i < lstPoints.Count; i++)
            {
                Vector3 p0 = lstPoints[i];
                if (i + 1 < lstPoints.Count)
                {
                    Vector3 p1 = lstPoints[i + 1];
                    s += Vector3.Distance(p0, p1);
                }
            }

            if (velocity > 0f) return s / velocity;
            return defaultValue;
        }
        public float MoveTowards(float current, float target, float maxDelta)
        {
            if (Mathf.Abs(target - current) <= maxDelta)
            {
                return target;
            }
            return current + Mathf.Sign(target - current) * maxDelta;
        }

        //https://javascript.info/bezier-curve
        /// <summary>
        /// Bezier for 2 control points.<br>P = (1-t) * P1 + t * P2</br> 
        /// </summary>
        public float Bezier2(float P1, float P2, float t)
        {
            float P = (1 - t) * P1 + t * P2;
            return P;
        }
        /// <summary>
        /// Bezier for 3 control points.<br>P = (1−t)^2 * P1 + 2 * (1−t) * t * P2 + t^2 * P3</br> 
        /// </summary>
        public float Bezier3(float P1, float P2, float P3, float t)
        {
            float P = Mathf.Pow((1 - t), 2) * P1 + 2 * (1 - t) * t * P2 + Mathf.Pow(t, 2) * P3;
            return P;
        }
        /// <summary>
        /// Bezier for 4 control points.<br>P = (1−t)^3 * P1 + 3 * (1−t)^2 * t * P2 + 3 * (1−t) * t^2 * P3 + t^3 * P4</br> 
        /// </summary>
        public float Bezier4(float P1, float P2, float P3, float P4, float t)
        {
            float P = Mathf.Pow((1 - t), 3) * P1 + 3 * Mathf.Pow((1 - t), 2) * t * P2 + 3 * (1 - t) * Mathf.Pow(t, 2) * P3 + Mathf.Pow(t, 3) * P4;
            return P;
        }
        public static Vector3 GenRandomVector3(Vector3 v3, float rangeX, float rangeY)
        {
            float x = Random.Range(v3.x - rangeX, v3.x + rangeY);
            float y = Random.Range(v3.y - rangeX, v3.y + rangeY);
            return new Vector3(x, y);
        }
        public static int GetPercentRateIndex(List<int> rates)
        {
            List<int> sample = new List<int>();
            int sampleCount = 100;
            for (int i = 0; i < rates.Count; i++)
            {
                int rate = rates[i];
                for (int j = 0; j < rate; j++) sample.Add(i);
            }

            if (sample.Count < sampleCount)
            {
                for (int i = sample.Count; i < sampleCount; i++)
                    sample.Add(-1);
            }

            int idx = (int)MinMaxLimit(Mathf.Floor(Random.value * sampleCount), 0, sampleCount - 1);
            return sample[idx];
        }
        public static float Remap(float x, float A, float B, float C, float D)
        {
            if ((B - A) == 0 || (D - C) == 0) return 0;
            float remappedValue = C + (x - A) / (B - A) * (D - C);
            return remappedValue;
        }
        public static Vector3 RotateDirection2D(float angle, Vector3 direction)
        {
            float radian = ToRadian(angle);
            return new Vector3(
                direction.x * Mathf.Cos(radian) - direction.y * Mathf.Sin(radian),
                direction.x * Mathf.Sin(radian) + direction.y * Mathf.Cos(radian));
        }
        public static List<T> Shuffle<T>(List<T> ret, int time = 1)
        {
            int num = ret.Count;
            while (num > 1)
            {
                num--;
                int k = RandomInt(0, num, time);
                T tmp = ret[k];
                ret[k] = ret[num];
                ret[num] = tmp;
            }
            return ret;
        }
        public static T[] Shuffle2<T>(T[] ret, int time = 1)
        {
            int num = ret.Length;
            while (num > 1)
            {
                num--;
                int k = RandomInt(0, num, time);
                T tmp = ret[k];
                ret[k] = ret[num];
                ret[num] = tmp;
            }
            return ret;
        }
        
        public static double TinhDienTichDaGiacV3(params Vector3[] points)
        {//tinh dien tich da giac bang cong thuc Shoelace:
            int n = points.Length;
            if (n < 3) return 0;

            double sum1 = 0;
            double sum2 = 0;

            for (int i = 0; i < n - 1; i++)
            {
                sum1 += points[i].x * points[i + 1].y;
                sum2 += points[i].y * points[i + 1].x;
            }

            // Cong them x_n*y_1 va y_n*x_1
            sum1 += points[n - 1].x * points[0].y;
            sum2 += points[n - 1].y * points[0].x;

            double area = System.Math.Abs(sum1 - sum2) / 2.0;            
            return area;
        }
        public static double TinhDienTichDaGiacV2(params Vector2[] points)
        {//tinh dien tich da giac bang cong thuc Shoelace:
            int n = points.Length;
            if (n < 3) return 0;

            double sum1 = 0;
            double sum2 = 0;

            for (int i = 0; i < n - 1; i++)
            {
                sum1 += points[i].x * points[i + 1].y;
                sum2 += points[i].y * points[i + 1].x;
            }

            // Cong them x_n*y_1 va y_n*x_1
            sum1 += points[n - 1].x * points[0].y;
            sum2 += points[n - 1].y * points[0].x;

            double area = System.Math.Abs(sum1 - sum2) / 2.0;
            return area;
        }
    }
}
