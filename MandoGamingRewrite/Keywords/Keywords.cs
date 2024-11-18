using R2API;

namespace MandoGamingRewrite.Keywords
{
    public static class Keywords
    {
        public static void Init()
        {
            LanguageAPI.Add("KEYWORD_ARC", "<style=cKeywordName>Arcing</style><style=cSub>Arc lighting up to 4 enemies for 30% damage per hit.</style>");
            LanguageAPI.Add("KEYWORD_FRICTIONLESS", "<style=cKeywordName>Frictionless</style><style=cSub>Suffers no damage falloff.</style>");
        }
    }
}