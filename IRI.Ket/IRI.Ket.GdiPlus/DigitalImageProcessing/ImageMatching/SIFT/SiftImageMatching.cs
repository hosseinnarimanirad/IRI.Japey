// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using IRI.Sta.DataStructures;
using IRI.Sta.DataStructures.CustomStructures;

namespace IRI.Ket.DigitalImageProcessing.ImageMatching;

public class SiftImageMatching
{
    List<ImageDescriptors> database;

    public SiftImageMatching(List<ImageDescriptors> database)
    {
        this.database = database;
    }

    public int FindMatch(ImageDescriptors image, double threshold)
    {
        List<IndexValue<double>> values = CalculateSimilariry(image, threshold);

        SortAlgorithm.Heapsort<IndexValue<double>>(ref values, SortDirection.Ascending);

        return values[0].Index;
    }

    //Lowe use threshold = 0.8
    public List<IndexValue<double>> CalculateSimilariry(ImageDescriptors image, double threshold)
    {
        List<IndexValue<double>> values = new List<IndexValue<double>>();

        for (int i = 0; i < database.Count; i++)
        {
            Dictionary<int, int> temp = Compare(this.database[i], image, threshold);

            values.Add(new IndexValue<double>(i, temp.Count));
        }

        return values;
    }


    public Dictionary<int, int> Compare(ImageDescriptors referenceImage, ImageDescriptors targetImage, double threshold)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();

        for (int i = 0; i < targetImage.Count; i++)
        {
            IndexValue<double>[] temp = new IndexValue<double>[referenceImage.Count];

            for (int j = 0; j < referenceImage.Count; j++)
            {
                temp[j] = new IndexValue<double>(j, Descriptor.CalculateAngle(referenceImage[j], targetImage[i]));
            }

            SortAlgorithm.Heapsort<IndexValue<double>>(ref temp, SortDirection.Descending);

            if (temp[0].Value.Equals(double.NaN))
            {
                throw new NotImplementedException();
            }

            if (temp[0].Value / temp[1].Value < threshold)
            {
                result.Add(i, temp[0].Index);
            }
            
        }

        return result;
    }
}
