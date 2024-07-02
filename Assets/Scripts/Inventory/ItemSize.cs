using UnityEngine;

public enum ItemSize
{
    Null = 0,
    A11Small = 1,

    [Header("One variable is equal to one")]
    A21OneMediumHorizontal = 2,
    A12OneMediumVertical = 3,

    A31OneLargeHorizontal = 4,
    A13OneLargeVertical = 5,

    A41OneBigHorizontal = 6,
    A14OneBigVertical = 7,

    A51OneHugeHorizontal = 8,
    A15OneHugeVertical = 9,

    [Header("One variable is equal to two")]
    A32TwoLargeHorizontal = 10,
    A23TwoLargeVertical = 11,

    A42TwoBigHorizontal = 12,
    A24TwoBigVertical = 13,

    A52TwoHugeHorizontal = 14,
    A25TwoHugeVertical = 15,

    [Header("Squares")]
    A22MedumScuare = 16,
    A33LargeScuare = 17,
    A44BigScuare = 18,
}       
