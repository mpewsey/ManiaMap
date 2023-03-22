using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A set of door code flags.
    /// </summary>
    [Flags]
    public enum DoorCode
    {
        /// No door code. This can match only with other no door codes.
        None = 0,
        /// The A flag.
        A = 1 << 0,
        /// The B flag.
        B = 1 << 1,
        /// The C flag.
        C = 1 << 2,
        /// The D flag.
        D = 1 << 3,
        /// The E flag.
        E = 1 << 4,
        /// The F flag.
        F = 1 << 5,
        /// The G flag.
        G = 1 << 6,
        /// The H flag.
        H = 1 << 7,
        /// The I flag.
        I = 1 << 8,
        /// The J flag.
        J = 1 << 9,
        /// The K flag.
        K = 1 << 10,
        /// The L flag.
        L = 1 << 11,
        /// The M flag.
        M = 1 << 12,
        /// The N flag.
        N = 1 << 13,
        /// The O flag.
        O = 1 << 14,
        /// The P flag.
        P = 1 << 15,
        /// The Q flag.
        Q = 1 << 16,
        /// The R flag.
        R = 1 << 17,
        /// The S flag.
        S = 1 << 18,
        /// The T flag.
        T = 1 << 19,
        /// The U flag.
        U = 1 << 20,
        /// The V flag.
        V = 1 << 21,
        /// The W flag.
        W = 1 << 22,
        /// The X flag.
        X = 1 << 23,
        /// The Y flag.
        Y = 1 << 24,
        /// The Z flag.
        Z = 1 << 25,
    }
}
