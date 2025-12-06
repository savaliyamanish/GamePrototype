
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public static class ExtensionMethods
{
   
    public static string ToShortAmount(this long amount)
    {
        if (amount / 10000000 >= 1)
        {
            float f = amount / 10000000f;
            if (f % 1 == 0)
                return f.ToString("N0") + " Cr";
            else
                return f.ToString("N1") + " Cr";

        }
        else if (amount / 100000 >= 1)
        {
            float f = amount / 100000f;
            if (f % 1 == 0)
                return f.ToString("N0") + " L";
            else
                return f.ToString("N1") + " L";
        }
        // else if (amount / 1000 >= 1) {
        // 	float f = amount / 1000f;
        // 	if(f%1==0)
        // 		return f.ToString ("N0") + "K";
        // 	else
        // 		return f.ToString ("N1") + "K";
        // }
        else
        {
            return amount.ToString("N0");
        }
    } //"₹ "
    public static void Shuffle<T>(this List<T> alpha)
    {

        for (int i = 0; i < alpha.Count; i++)
        {
            T temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }
    public static string GetToString<T>(this List<T> alpha,string separator=" ")
    {
        StringBuilder stringBuilder=new StringBuilder();
           stringBuilder.Append($"[{alpha.Count}]");
        for (int i = 0; i < alpha.Count; i++)
        {
           stringBuilder.Append(separator);
           stringBuilder.Append(alpha[i].ToString());
        }
        return stringBuilder.ToString();
    }
    public static void Shuffle<T>(this List<T> alpha, int seed)
    {
        System.Random random = new System.Random(seed);
        for (int i = 0; i < alpha.Count; i++)
        {
            T temp = alpha[i];
            int randomIndex = random.Next(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }
    public static T GetClamp<T>(this List<T> lst, long index)
    {
        return lst.GetClamp((int)index);
    }
    public static T GetClamp<T>(this List<T> lst, int index)
    {
        if (lst == null || lst.Count <= 0)
        {
            return default(T);
        }
        return lst[Mathf.Clamp(index, 0, lst.Count - 1)];
    }
    public static T GetRandom<T>(this List<T> alpha)
    {
        if (alpha == null || alpha.Count == 0)
            return default(T);
        return alpha[Random.Range(0, alpha.Count)];
    }
    public static int GetRandomIndex<T>(this List<T> alpha)
    {
        if (alpha == null || alpha.Count == 0)
            return -1;
        return Random.Range(0, alpha.Count);
    }
    public static T GetRandomWithFilter<T>(this List<T> alpha, System.Predicate<T> filter)
    {
        alpha = alpha.FindAll(filter);
        if (alpha == null || alpha.Count == 0)
            return default(T);
        return alpha[Random.Range(0, alpha.Count)];
    }
   

    public static T GetRandom<T>(this List<T> alpha, int seed)
    {
        if (alpha == null || alpha.Count == 0)
            return default(T);
        System.Random random = new System.Random(seed);
        return alpha[random.Next(0, alpha.Count)];
    }
    public static int GetRandomIndex<T>(this List<T> alpha, int seed)
    {
        if (alpha == null || alpha.Count == 0)
            return -1;
        System.Random random = new System.Random(seed);
        return random.Next(0, alpha.Count);
    }

    public static Texture2D CropCenterSqure(this Texture2D texture)
    {
        int size = 0, x = 0, y = 0;
        if (texture.width > texture.height)
        {
            x = (texture.width - texture.height) / 2;
            size = texture.height;
        }
        else
        {
            y = (texture.height - texture.width) / 2;
            size = texture.width;
        }

        Color[] pixels = texture.GetPixels(x, y, size, size, 0);
        Texture2D cropped = new Texture2D(size, size, TextureFormat.ARGB32, false);
        cropped.SetPixels(0, 0, cropped.width, cropped.height, pixels, 0);
        cropped.Apply();
        return cropped;
    }
    public static Sprite ToSprite(this Texture2D texture)
    {
        if (texture == null) return null;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * .5f);
    }
    public static Rect getWorldRect(this RectTransform transform)
    {
        Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
    public static string ToFirstUpper(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return string.Empty;

        char[] a = s.ToCharArray();
        a[0] = char.ToUpper(a[0]);
        return new string(a);

    }
    public static string ToOrdinal(this long value)
    {
        string extension = "th";
        long last_digits = value % 100;
        if (last_digits < 11 || last_digits > 13)
        {
            switch (last_digits % 10)
            {
                case 1:
                    extension = "st";
                    break;
                case 2:
                    extension = "nd";
                    break;
                case 3:
                    extension = "rd";
                    break;
            }
        }

        return extension;
    }
    public static string WrapText(this string sentence, int columnWidth)
    {

        string[] words = sentence.Split(' ');

        System.Text.StringBuilder newSentence = new System.Text.StringBuilder();

        string line = "";
        for (int i = 0; i < words.Length; i++)
        {
            if ((line + words[i]).Length > columnWidth)
            {
                newSentence.AppendLine(line);
                line = "";
            }

            line += string.Format("{0} ", words[i]);
        }

        if (line.Length > 0)
            newSentence.Append(line);

        return newSentence.ToString();
    }
    public static Texture2D AlphaBlend(this Texture2D[] aBottom)
    {
        Texture2D main = aBottom[0];

        for (int j = 1; j < aBottom.Length; j++)
        {
            Color[] bData = main.GetPixels();
            Color[] tData = aBottom[j].GetPixels();

            int count = bData.Length;
            Color[] rData = new Color[count];

            for (int i = 0; i < count; i++)
            {
                Color B = bData[i];
                Color T = tData[i];
                float srcF = T.a;
                float destF = 1f - T.a;
                float alpha = srcF + destF * B.a;
                Color R = (T * srcF + B * B.a * destF) / alpha;
                R.a = alpha;
                rData[i] = R;
            }

            main.SetPixels(rData);
            main.Apply();
        }
        return main;
    }
        public static string TimeOnlyString(this double t, bool ignoreNagetive = true)
    {
        if (t < 0 && ignoreNagetive)
            t = 0;
        long sec = (long)t;
        string s = "";
        s += (Mathf.FloorToInt(sec / 60f) % 60).ToString("00") + ":";
        s += (sec % 60).ToString("00");
        return s;
    }
    public static string TimeAgo(this System.DateTime dateTime)
    {
        string result = string.Empty;
        var timeSpan = System.DateTime.Now.Subtract(dateTime);

        if (timeSpan <= System.TimeSpan.FromSeconds(60))
        {
            result = string.Format("{0} seconds ago", timeSpan.Seconds);
        }
        else if (timeSpan <= System.TimeSpan.FromMinutes(60))
        {
            result = timeSpan.Minutes > 1 ?
                string.Format("about {0} minutes ago", timeSpan.Minutes) :
                "about a minute ago";
        }
        else if (timeSpan <= System.TimeSpan.FromHours(24))
        {
            result = timeSpan.Hours > 1 ?
                string.Format("about {0} hours ago", timeSpan.Hours) :
                "about an hour ago";
        }
        else if (timeSpan <= System.TimeSpan.FromDays(30))
        {
            result = timeSpan.Days > 1 ?
                string.Format("about {0} days ago", timeSpan.Days) :
                "yesterday";
        }
        else if (timeSpan <= System.TimeSpan.FromDays(365))
        {
            result = timeSpan.Days > 30 ?
                string.Format("about {0} months ago", timeSpan.Days / 30) :
                "about a month ago";
        }
        else
        {
            result = timeSpan.Days > 365 ?
                string.Format("about {0} years ago", timeSpan.Days / 365) :
                "about a year ago";
        }

        return result;
    }
    public static string getShortName(this string PlayerName)
    {
        string[] sortName = PlayerName.Split(new char[0]);
        return sortName[0];
    }

    public static bool IsEnglish(this string inputstring)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"[A-Za-z0-9 .,-=+(){}\[\]\\]");
        System.Text.RegularExpressions.MatchCollection matches = regex.Matches(inputstring);

        if (matches.Count.Equals(inputstring.Length))
            return true;
        else
            return false;
    }
    public static string GetEnglish(this string inputstring)
    {
        return inputstring.IsEnglish() ? inputstring : "";
    }
    public static void DestroyAllChild(this Transform t)
    {
        if (t != null && t.childCount > 0)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(t.GetChild(i).gameObject);
            }
        }
    }

    public static void DestroyAllChildImmediate(this Transform t)
    {
        if (t != null && t.childCount > 0)
        {
            for (int i = t.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(t.GetChild(i).gameObject);
            }
        }
    }
    public static Vector3[] GetArcPath(Vector3 start, Vector3 end, float height, int pathCount)
    {
        Vector3[] posArr = new Vector3[pathCount];
        Vector3 center = (start + end) * 0.5f;
        // center =center+ Quaternion.Euler(0,0,Vector3.Angle(start,end))*Vector3.up*-height*2; for 2D

        Vector3 temp = Vector3.Cross(center - end, Vector3.forward);
        temp.Normalize();
        center = height * temp + center;

        for (int i = 0; i < pathCount; i++)
        {
            Vector3 m1 = Vector3.Lerp(start, center, (float)i / (pathCount - 1));
            Vector3 m2 = Vector3.Lerp(center, end, (float)i / (pathCount - 1));
            posArr[i] = Vector3.Lerp(m1, m2, (float)i / (pathCount - 1));
        }
        return posArr;
    }
    public static string ToTitleCase(this string text, int sizePercent = 140)
    {
        string[] word = text.Split(' ');
        for (int i = 0; i < word.Length; i++)
        {
            word[i] = word[i].ToUpper();
            word[i] = "<size=" + sizePercent + "%>" + word[i].Substring(0, 1) + "</size>" + word[i].Substring(1, word[i].Length - 1);
        }
        return string.Join(" ", word);

    }
 
    public static LTDescr UIColorTween(this GameObject go, Color color, float t)
    {
        Graphic graphics = go.GetComponent<Graphic>();
        if (graphics != null)
        {
            return LeanTween.color(go, color, t).setOnUpdateColor((Color c) =>
            {
                graphics.color = c;
            });
        }
        return null;
    }

    public static float ConvertToNum(this string text)
    {
        float f = 0;
        string numstr = "0";
        char[] arr = text.ToCharArray();
        for (int i = 0; i < arr.Length; i++)
        {
            if (char.IsDigit(arr[i]) || (arr[i] == '.' && !numstr.Contains(".")))
            {
                numstr += arr[i];
            }
        }
        if (!float.TryParse(numstr, out f))
        {
            Debug.Log("Convert To Num Failed : " + text);
        }
        return f;
    }
    public static LTDescr UIAlphaTween(this MonoBehaviour monoB, float alpha, float animTime, bool withChild = false)
    {
        return UIAlphaTween(monoB.gameObject, alpha, animTime, withChild);
    }
    public static LTDescr UIAlphaTween(this GameObject go, float alpha, float animTime, bool withChild = false)
    {
        if (withChild)
        {
            var allChild = go.GetComponentsInChildren<Graphic>();
            if (allChild != null)
            {
                var allChildColor = new Color[allChild.Length];
                for (int i = 0; i < allChild.Length; i++)
                {
                    allChildColor[i] = allChild[i].color;
                }
                return LeanTween.value(go, 0, 1, animTime).setOnUpdate((float f) =>
                {
                    for (int i = 0; i < allChild.Length; i++)
                    {
                        Color c = allChildColor[i];
                        c.a = Mathf.Lerp(allChildColor[i].a, alpha, f);
                        allChild[i].color = c;
                    }
                });
            }
        }
        else
        {
            Graphic graphics = go.GetComponent<Graphic>();
            if (graphics != null)
            {
                Color c = graphics.color;
                return LeanTween.value(go, c.a, alpha, animTime).setOnUpdate((float f) =>
                {
                    c.a = f;
                    graphics.color = c;
                });
            }
        }
        return null;
    }
    public static LTDescr SetValueWithAnim(this Slider slider, float value, float animTime)
    {
        return LeanTween.value(slider.gameObject, slider.value, value, animTime).setOnUpdate((float f) => slider.value = f);
    }

    public static void SetUIAlpha(this MonoBehaviour monoB, float alpha, bool withChild = false)
    {
        SetUIAlpha(monoB.gameObject, alpha, withChild);
    }
    public static void SetUIAlpha(this GameObject go, float alpha, bool withChild = false)
    {
        if (withChild)
        {
            var allChild = go.GetComponentsInChildren<Graphic>();
            if (allChild != null)
            {
                for (int i = 0; i < allChild.Length; i++)
                {
                    Color c = allChild[i].color;
                    c.a = alpha;
                    allChild[i].color = c;
                }
            }
        }
        else
        {
            Graphic graphics = go.GetComponent<Graphic>();
            if (graphics != null)
            {
                Color c = graphics.color;
                c.a = alpha;
                graphics.color = c;
            }
        }
    }
    public static bool IsValidEmail(this string email)
    {
        string MatchEmailPattern =
            @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        //@" ([a-zA-Z]+[0-9]{6,})@"
        if (email != null)
            return System.Text.RegularExpressions.Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }
    public static Vector3 GetNearestPointOnNavMesh(this Vector3 position, float maxDistance = 5)
    {
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(position, out hit, maxDistance, 1))
        {
            return hit.position; // Return the nearest point on the NavMesh
        }
        else
        {
            Debug.Log($"No NavMesh found near {position}. Consider increasing the maxDistance.");
            return position; // Fallback to the original position
        }
    }


    public static void ScrollToItemHorizontal(this ScrollRect scrollRect, RectTransform targetItem)
    {
        Canvas.ForceUpdateCanvases(); // Make sure layout is up to date

        RectTransform content = scrollRect.content;
        RectTransform viewport = scrollRect.viewport;

        // Calculate position of the item relative to the content
        Vector2 itemLocalPos = (Vector2)content.InverseTransformPoint(content.position) -
                            (Vector2)content.InverseTransformPoint(targetItem.position);

        float contentWidth = content.rect.width;
        float viewportWidth = viewport.rect.width;

        // Calculate normalized scroll position (0 = left, 1 = right)
        float normalizedPos = Mathf.Clamp01((-itemLocalPos.x - (viewportWidth * .5f)) / (contentWidth - viewportWidth));
        scrollRect.horizontalNormalizedPosition = normalizedPos;
    }
}