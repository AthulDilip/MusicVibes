using System;
using System.Collections.Generic;
using System.Linq;

namespace MusicVibes
{
    public class MappingService
    {
        public MappingService()
        {
        }

        public static List<string> getGenreFromMood(Dictionary<string, double> moodDict)
        {
            List<string> genreList = new List<string>();

            moodDict["Neutral"] -= 0.6;
            double maxVal = moodDict.Values.Max();

            string maxKey = moodDict.Where(x => x.Value.Equals(maxVal)).FirstOrDefault().Key;

            if (maxKey != null)
            {
                if (maxKey == "Anger")
                {
                    genreList.Add(CategoryConstValues.PunkRock);
                    genreList.Add(CategoryConstValues.Peace);
                    genreList.Add(CategoryConstValues.Rap);
                }
                else if (maxKey == "Contempt")
                {
                    genreList.Add(CategoryConstValues.Rap);
                    genreList.Add(CategoryConstValues.Rock);
                }
                else if (maxKey == "Disgust" || maxKey == "Sadness" || maxKey == "Fear")
                {
                    genreList.Add(CategoryConstValues.Sad);
                    genreList.Add(CategoryConstValues.Peace);
                }
                else if (maxKey == "Happiness" || maxKey == "Surprise")
                {
                    genreList.Add(CategoryConstValues.Happy);
                    genreList.Add(CategoryConstValues.Rock);
                }
                else if (maxKey == "Neutral")
                {
                    genreList.Add(CategoryConstValues.Pop);
                    genreList.Add(CategoryConstValues.Happy);
                    genreList.Add(CategoryConstValues.Rock);
                }

            }

            return genreList;
        }
    }
}
