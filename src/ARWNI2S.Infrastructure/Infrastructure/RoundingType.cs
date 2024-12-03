namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Rounding type
    /// </summary>
    public enum RoundingType
    {
        /// <summary>
        /// Default rounding (Match.Round(num, 2))
        /// </summary>
        Rounding001 = 0,

        /// <summary>
        /// <![CDATA[Values are rounded up to the nearest multiple of .05 for values ending in: .03 & .04 round to .05; and, .08 & .09 round to .10]]>
        /// </summary>
        Rounding005Up = 10,

        /// <summary>
        /// <![CDATA[Values are rounded down to the nearest multiple of .05 for values ending in: .01 & .02 to 0; and, .06 & .07 to .05]]>
        /// </summary>
        Rounding005Down = 20,

        /// <summary>
        /// <![CDATA[Round up to the nearest .1 value for values ending in .05]]>
        /// </summary>
        Rounding01Up = 30,

        /// <summary>
        /// <![CDATA[Round down to the nearest .1 value for values ending in .05]]>
        /// </summary>
        Rounding01Down = 40,

        /// <summary>
        /// <![CDATA[Values ending in .01–.24 round down to .0
        /// Values ending in .25–.49 round up to .5
        /// Values ending in .51–.74 round down to .5
        /// Values ending in .75–.99 round up to the next integer]]>
        /// </summary>
        Rounding05 = 50,

        /// <summary>
        /// <![CDATA[Values ending in .01–.49 round down to .0
        /// Values ending in .50–.99 round up to the next integer
        /// For example, Absolute scalar values]]>
        /// </summary>
        Rounding1 = 60,

        /// <summary>
        /// <![CDATA[Values ending in .01–.99 round up to the next integer]]>
        /// </summary>
        Rounding1Up = 70
    }
}
