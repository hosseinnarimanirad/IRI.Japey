// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
namespace IRI.Ket.Geometry
{
    //[Flags]
    public enum TriangleRelation
    {
        Nothing = 0,
        MeetAtFirst = 1,
        MeetAtSecond = 2,
        MeetAtThird = 4,
        //Meet = MeetAtFirst | MeetAtSecond | MeetAtThird,
        FirstSecondNeighbour = 3,
        SecondThirdNeighbour = 6,
        ThirdFirstNeighbour = 5,
        //Neighbour = FirstSecondNeighbour | SecondThirdNeighbour | ThirdFirstNeighbour,
        Equal =7,
    }
}
