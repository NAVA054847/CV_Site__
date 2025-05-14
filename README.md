# CV_Site__

# GitHub Portfolio Web API

## תיאור הפרויקט

פרויקט זה הוא Web API שנבנה באמצעות ASP.NET Core ומטרתו לספק מידע מ-GitHub בצורה מובנית ונוחה לצריכה על ידי צד לקוח (Frontend). ה-API מאפשר לבצע את הפעולות הבאות:

**קבלת רשימת Repositorys של משתמש:** שליפת רשימה של כל ה-repositories הציבוריים של משתמש GitHub ספציפי.
**קבלת פרטי Repository ספציפי:** שליפת פרטים מלאים על repository ספציפי, כולל שם הבעלים ושם ה-repository.
**חיפוש Repositorys:** ביצוע חיפוש גלובלי ב-GitHub על פני כל ה-repositories הפומביים, עם אפשרות סינון לפי שם, שפת פיתוח ושם משתמש.
**שיפור ביצועים באמצעות Cache:** רשימת ה-repositories של משתמש נשמרת ב-Cache למשך מספר דקות כדי לשפר את מהירות התגובה ולהפחית את העומס על GitHub API.

פרויקט זה נועד לשמש כ-Backend עבור אפליקציית פורטפוליו המציגה פרויקטים מ-GitHub.

## טכנולוגיות בשימוש

**ASP.NET Core:** פלטפורמת פיתוח ווב מודרנית וחוצת פלטפורמות של Microsoft.
**C#:** שפת התכנות העיקרית.
**Octokit.NET:** ספריית .NET GitHub API Client הרשמית.
**Microsoft.Extensions.Configuration.UserSecrets:** לניהול סודות פיתוח (כגון טוקן GitHub).
**Microsoft.Extensions.Caching.Memory:** ליישום Cache בזיכרון.
**HttpClient:** לביצוע בקשות HTTP ל-GitHub API.

## דרישות מקדימות

לפני שתתחיל, ודא שהמערכת שלך עומדת בדרישות הבאות:

**.NET SDK:** התקן את ה-.NET SDK העדכני ביותר (גרסה 6.0 ומעלה מומלצת) מהאתר הרשמי של [.NET](https://dotnet.microsoft.com/download).
**Visual Studio (מומלץ) או Visual Studio Code:** סביבת פיתוח משולבת (IDE) מומלצת לפיתוח .NET. Visual Studio Code היא חלופה קלת משקל וחינמית עם תמיכה מצוינת ב-#C.
**Git:** התקן את Git אם עדיין לא מותקן במערכת שלך ([https://git-scm.com/downloads](https://git-scm.com/downloads)).

## הוראות התקנה

1.  **שיבוט (Clone) הפרויקט:**
    פתח את הטרמינל או את כלי ה-Git שלך ונווט לתיקייה שבה תרצה לשמור את הפרויקט. הרץ את הפקודה הבאה:

   
bash
    git clone <כתובת_ה-URL_של_הפרויקט_ב-GitHub>
   

    (החלף את <כתובת_ה-URL_של_הפרויקט_ב-GitHub> בכתובת ה-URL של הפרויקט שלך ב-GitHub).

2.  **ניווט לתיקיית הפרויקט:**
    לאחר השיבוט, נווט לתיקייה הראשית של הפרויקט:

   
bash
    cd GitHubPortfolio
   

3.  **שחזור חבילות NuGet:**
    אם אתה משתמש ב-Visual Studio, פתח את קובץ ה-Solution (.sln). Visual Studio ישחזר באופן אוטומטי את חבילות ה-NuGet הנדרשות.
    אם אתה משתמש ב-Visual Studio Code או ב-CLI של .NET, הרץ את הפקודה הבאה בתיקייה הראשית של הפרויקט (בתוך תיקיית ה-WebAPI):

   
bash
    dotnet restore
   

4.  **הגדרת טוקן GitHub (סודות משתמש):**
    כדי שהאפליקציה תוכל לתקשר עם GitHub API, תצטרך ליצור טוקן גישה אישי (Personal Access Token) מ-GitHub. בצע את השלבים הבאים:
    * עבור להגדרות המפתח האישי שלך ב-GitHub ([https://github.com/settings/tokens](https://github.com/settings/tokens)).
    * לחץ על "Generate new token".
    * תן שם לטוקן (למשל, "GitHubPortfolio").
    * בחר את הסקופ המתאים (במקרה של פרויקט זה, סקופ public_repo עשוי להספיק לקריאת מידע פומבי).
    * לחץ על "Generate token". **העתק את הטוקן שנוצר ושמור אותו במקום בטוח.**
    * בתוך פרויקט ה-WebAPI שלך, הפעל את הפקודה הבאה בטרמינל (אם עדיין לא עשית זאת, נווט לתיקיית הפרויקט של ה-WebAPI):

       
bash
        dotnet user-secrets init
       

        זה יאתחל את תמיכת סודות המשתמש עבור הפרויקט שלך.

    * לאחר מכן, הגדר את הטוקן שלך כסוד משתמש באמצעות הפקודה הבאה (החלף את <הטוקן_שלך> בטוקן שהעתקת):

       
bash
        dotnet user-secrets set "GitHubToken" "<הטוקן_שלך>"
       

        (ב-Visual Studio, תוכל גם ללחוץ לחיצה ימנית על פרויקט ה-WebAPI ולבחור "Manage User Secrets" ולעדכן שם את הטוקן בפורמט JSON: {"GitHubToken": "<הטוקן_שלך>"}).

## הרצת ה-API

1.  **הרצה באמצעות Visual Studio:**
    פתח את קובץ ה-Solution (.sln) ב-Visual Studio. ודא שפרויקט ה-WebAPI מוגדר כפרויקט ההפעלה (לחץ לחיצה ימנית על הפרויקט ובחר "Set as Startup Project"). לחץ על כפתור ההרצה (Play) או לחץ על F5.

2.  **הרצה באמצעות .NET CLI:**
    פתח את הטרמינל ונווט לתיקייה של פרויקט ה-WebAPI. הרץ את הפקודה הבאה:

   
bash
    dotnet run
   

    ה-API יתחיל לפעול, ובדרך כלל תוכל לראות הודעה בקונסול המציינת את כתובות ה-URL שבהן הוא מאזין (למשל, http://localhost:5000 או https://localhost:5001).

## נקודות קצה (Endpoints) של ה-API

לאחר שה-API רץ, תוכל לגשת לנקודות הקצה הבאות:

**קבלת רשימת Repositorys של משתמש:**
    GET /api/github/{username}/repositories
    (החלף {username} בשם המשתמש של GitHub).

**קבלת פרטי Repository ספציפי:**
    GET /api/github/{owner}/{repoName}
    (החלף {owner} בשם המשתמש של הבעלים ו-{repoName} בשם ה-repository).

**חיפוש Repositorys:**
    GET /api/github/search?name={name}&language={language}&user={user}
    (כל הפרמטרים הם אופציונליים).

## שימוש ב-API

תוכל להשתמש בכלי כמו דפדפן אינטרנט (לGET פשוטים) או כלי כמו Postman או curl כדי לשלוח בקשות לנקודות הקצה של ה-API ולקבל תגובות JSON.

## תרומה

אם אתה מעוניין לתרום לפרויקט זה, אנא צור קשר או פתח בקשת משיכה (Pull Request) עם השינויים שלך.



