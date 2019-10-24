// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System
{
    internal unsafe readonly struct nint
    {
        private readonly byte* _value;

        private nint(byte* value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(nint value) => (IntPtr)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(void* value) => new nint((byte*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* operator +(byte* value, nint addend)
        {
            if (IntPtr.Size == 4)
            {
                return value + (int)addend._value;
            }
            else
            {
                return value + (long)addend._value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint operator >>(nint value, int shift)
        {
            if (IntPtr.Size == 4)
            {
                return new nint((byte*)((int)value._value >> shift)); // sign-extend
            }
            else
            {
                return new nint((byte*)((long)value._value >> shift)); // sign-extend
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nint(int value)
        {
            if (IntPtr.Size == 4)
            {
                return new nint((byte*)value);
            }
            else
            {
                return new nint((byte*)(long)value); // sign-extend
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nint(sbyte value)
        {
            if (IntPtr.Size == 4)
            {
                return new nint((byte*)(int)value); // sign-extend
            }
            else
            {
                return new nint((byte*)(long)value); // sign-extend
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(nint left, int right)
        {
            if (IntPtr.Size == 4)
            {
                return (int)left._value >= right;
            }
            else
            {
                return (long)left._value >= (long)right; // sign-extend
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(nint left, int right)
        {
            if (IntPtr.Size == 4)
            {
                return (int)left._value <= right;
            }
            else
            {
                return (long)left._value <= (long)right; // sign-extend
            }
        }
    }

    internal unsafe readonly struct nuint
    {
        private readonly byte* _value;

        private nuint(byte* value)
        {
            _value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IntPtr(nuint value) => (IntPtr)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator nuint(uint value) => new nuint((byte*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator nuint(void* value) => new nuint((byte*)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator byte*(nuint value) => value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator char*(nuint value) => (char*)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator int(nuint value) => (int)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator uint(nuint value) => (uint)value._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator +(nuint value, int addend) => new nuint(value._value + addend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator +(nuint value, uint addend) => new nuint(value._value + addend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator +(nuint value, nuint addend)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint(value._value + (uint)addend._value);
            }
            else
            {
                return new nuint(value._value + (ulong)addend._value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* operator +(byte* value, nuint addend)
        {
            if (IntPtr.Size == 4)
            {
                return value + (uint)addend._value;
            }
            else
            {
                return value + (ulong)addend._value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char* operator +(char* value, nuint addend)
        {
            if (IntPtr.Size == 4)
            {
                return value + (uint)addend._value;
            }
            else
            {
                return value + (ulong)addend._value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator ++(nuint value) => value + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator --(nuint value) => value - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator -(nuint value, int addend) => new nuint(value._value - addend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator -(nuint value, uint addend) => new nuint(value._value - addend);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator -(nuint value, nuint addend)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint(value._value - (uint)addend._value);
            }
            else
            {
                return new nuint(value._value - (ulong)addend._value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(nuint left, nuint right) => left._value < right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator <=(nuint left, nuint right) => left._value <= right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(nuint left, nuint right) => left._value > right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator >=(nuint left, nuint right) => left._value >= right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(nuint left, nuint right) => left._value == right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(nuint left, nuint right) => left._value != right._value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator ~(nuint value)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint((byte*)~(uint)value._value);
            }
            else
            {
                return new nuint((byte*)~(ulong)value._value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator &(nuint left, nuint right)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint((byte*)((uint)left._value & (uint)right._value));
            }
            else
            {
                return new nuint((byte*)((ulong)left._value & (ulong)right._value));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator %(nuint left, uint right)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint((byte*)((uint)left._value % right));
            }
            else
            {
                return new nuint((byte*)((ulong)left._value % right));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nuint operator /(nuint left, uint right)
        {
            if (IntPtr.Size == 4)
            {
                return new nuint((byte*)((uint)left._value / right));
            }
            else
            {
                return new nuint((byte*)((ulong)left._value / right));
            }
        }

        public override bool Equals(object obj) => throw new NotImplementedException();

        public override int GetHashCode() => throw new NotImplementedException();
    }
}
