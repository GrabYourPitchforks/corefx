// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace System
{
    [StackTraceHidden]
    internal static class ThrowHelper
    {
        internal static void ThrowArgumentNullException(ExceptionArgument argument) { throw CreateArgumentNullException(argument); }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentNullException(ExceptionArgument argument) { return new ArgumentNullException(argument.ToString()); }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRangeException()
        {
            throw new ArgumentOutOfRangeException();
        }

        internal static void ThrowInvalidOperationException() { throw CreateInvalidOperationException(); }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateInvalidOperationException() { return new InvalidOperationException(); }


        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument) { throw CreateArgumentOutOfRangeException(argument); }
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Exception CreateArgumentOutOfRangeException(ExceptionArgument argument) { return new ArgumentOutOfRangeException(argument.ToString()); }

        [DoesNotReturn]
        internal static void ThrowArgumentException_DestinationTooShort()
        {
            throw new ArgumentException("Destination too short.", paramName: "destination");
        }

        [DoesNotReturn]
        internal static void ThrowArgumentException_CannotExtractScalar(ExceptionArgument argument)
        {
            throw new ArgumentException("Cannot extract scalar.", paramName: argument.ToString());
        }

        [DoesNotReturn]
        internal static void ThrowArgumentOutOfRange_IndexException()
        {
            ThrowArgumentOutOfRangeException(ExceptionArgument.index);
        }

        [DoesNotReturn]
        internal static void ThrowLengthArgumentOutOfRange_ArgumentOutOfRange_NeedNonNegNum()
        {
            ThrowArgumentOutOfRangeException(ExceptionArgument.length);
        }

        [DoesNotReturn]
        internal static void ThrowValueArgumentOutOfRange_NeedNonNegNumException()
        {
            ThrowArgumentOutOfRangeException(ExceptionArgument.value);
        }
    }

    internal enum ExceptionArgument
    {
        obj,
        dictionary,
        array,
        info,
        key,
        text,
        values,
        value,
        startIndex,
        task,
        bytes,
        byteIndex,
        byteCount,
        ch,
        chars,
        charIndex,
        charCount,
        s,
        input,
        ownedMemory,
        list,
        index,
        capacity,
        collection,
        item,
        converter,
        match,
        count,
        action,
        comparison,
        exceptions,
        exception,
        pointer,
        start,
        format,
        culture,
        comparer,
        comparable,
        source,
        state,
        length,
        comparisonType,
        manager,
        sourceBytesToCopy,
        callBack,
        creationOptions,
        function,
        scheduler,
        continuationAction,
        continuationFunction,
        tasks,
        asyncResult,
        beginMethod,
        endMethod,
        endFunction,
        cancellationToken,
        continuationOptions,
        delay,
        millisecondsDelay,
        millisecondsTimeout,
        stateMachine,
        timeout,
        type,
        sourceIndex,
        sourceArray,
        destinationIndex,
        destinationArray,
        pHandle,
        other,
        newSize,
        lowerBounds,
        lengths,
        len,
        keys,
        indices,
        index1,
        index2,
        index3,
        length1,
        length2,
        length3,
        endIndex,
        elementType,
        arrayIndex,
        year,
    }
}
