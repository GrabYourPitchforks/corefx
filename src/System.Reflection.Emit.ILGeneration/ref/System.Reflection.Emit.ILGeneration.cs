// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------
// Changes to this file must follow the http://aka.ms/api-review process.
// ------------------------------------------------------------------------------

namespace System.Reflection.Emit
{
    public partial class CustomAttributeBuilder
    {
        public CustomAttributeBuilder(System.Reflection.ConstructorInfo con, object[] constructorArgs) { }
        public CustomAttributeBuilder(System.Reflection.ConstructorInfo con, object[] constructorArgs, System.Reflection.FieldInfo[] namedFields, object[] fieldValues) { }
        public CustomAttributeBuilder(System.Reflection.ConstructorInfo con, object[] constructorArgs, System.Reflection.PropertyInfo[] namedProperties, object[] propertyValues) { }
        public CustomAttributeBuilder(System.Reflection.ConstructorInfo con, object[] constructorArgs, System.Reflection.PropertyInfo[] namedProperties, object[] propertyValues, System.Reflection.FieldInfo[] namedFields, object[] fieldValues) { }
    }
    public partial struct EventToken
    {
        public static readonly System.Reflection.Emit.EventToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.EventToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.EventToken a, System.Reflection.Emit.EventToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.EventToken a, System.Reflection.Emit.EventToken b) { throw null; }
    }
    public partial struct FieldToken
    {
        public static readonly System.Reflection.Emit.FieldToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.FieldToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.FieldToken a, System.Reflection.Emit.FieldToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.FieldToken a, System.Reflection.Emit.FieldToken b) { throw null; }
    }
    public partial class ILGenerator
    {
        internal ILGenerator() { }
        public virtual int ILOffset { get { throw null; } }
        public virtual void BeginCatchBlock(System.Type exceptionType) { }
        public virtual void BeginExceptFilterBlock() { }
        public virtual System.Reflection.Emit.Label BeginExceptionBlock() { throw null; }
        public virtual void BeginFaultBlock() { }
        public virtual void BeginFinallyBlock() { }
        public virtual void BeginScope() { }
        public virtual System.Reflection.Emit.LocalBuilder DeclareLocal(System.Type localType) { throw null; }
        public virtual System.Reflection.Emit.LocalBuilder DeclareLocal(System.Type localType, bool pinned) { throw null; }
        public virtual System.Reflection.Emit.Label DefineLabel() { throw null; }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, byte arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, double arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, short arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, int arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, long arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.ConstructorInfo con) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.Label label) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.Label[] labels) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.LocalBuilder local) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.Emit.SignatureHelper signature) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.FieldInfo field) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Reflection.MethodInfo meth) { }
        [System.CLSCompliantAttribute(false)]
        public void Emit(System.Reflection.Emit.OpCode opcode, sbyte arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, float arg) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, string str) { }
        public virtual void Emit(System.Reflection.Emit.OpCode opcode, System.Type cls) { }
        public virtual void EmitCall(System.Reflection.Emit.OpCode opcode, System.Reflection.MethodInfo methodInfo, System.Type[] optionalParameterTypes) { }
        public virtual void EmitCalli(System.Reflection.Emit.OpCode opcode, System.Reflection.CallingConventions callingConvention, System.Type returnType, System.Type[] parameterTypes, System.Type[] optionalParameterTypes) { }
        public virtual void EmitWriteLine(System.Reflection.Emit.LocalBuilder localBuilder) { }
        public virtual void EmitWriteLine(System.Reflection.FieldInfo fld) { }
        public virtual void EmitWriteLine(string value) { }
        public virtual void EndExceptionBlock() { }
        public virtual void EndScope() { }
        public virtual void MarkLabel(System.Reflection.Emit.Label loc) { }
        public virtual void ThrowException(System.Type excType) { }
        public virtual void UsingNamespace(string usingNamespace) { }
    }
    public readonly partial struct Label : System.IEquatable<System.Reflection.Emit.Label>
    {
        private readonly int _dummyPrimitive;
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.Label obj) { throw null; }
        public override int GetHashCode() { throw null; }
        public static bool operator ==(System.Reflection.Emit.Label a, System.Reflection.Emit.Label b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.Label a, System.Reflection.Emit.Label b) { throw null; }
    }
    public sealed partial class LocalBuilder : System.Reflection.LocalVariableInfo
    {
        internal LocalBuilder() { }
        public override bool IsPinned { get { throw null; } }
        public override int LocalIndex { get { throw null; } }
        public override System.Type LocalType { get { throw null; } }
    }
    public partial struct MethodToken
    {
        public static readonly System.Reflection.Emit.MethodToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.MethodToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.MethodToken a, System.Reflection.Emit.MethodToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.MethodToken a, System.Reflection.Emit.MethodToken b) { throw null; }
    }
    public partial class ParameterBuilder
    {
        internal ParameterBuilder() { }
        public virtual int Attributes { get { throw null; } }
        public bool IsIn { get { throw null; } }
        public bool IsOptional { get { throw null; } }
        public bool IsOut { get { throw null; } }
        public virtual string Name { get { throw null; } }
        public virtual int Position { get { throw null; } }
        public virtual System.Reflection.Emit.ParameterToken GetToken() { throw null; }
        public virtual void SetConstant(object defaultValue) { }
        public void SetCustomAttribute(System.Reflection.ConstructorInfo con, byte[] binaryAttribute) { }
        public void SetCustomAttribute(System.Reflection.Emit.CustomAttributeBuilder customBuilder) { }
    }
    public partial struct ParameterToken
    {
        public static readonly System.Reflection.Emit.ParameterToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.ParameterToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.ParameterToken a, System.Reflection.Emit.ParameterToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.ParameterToken a, System.Reflection.Emit.ParameterToken b) { throw null; }
    }
    public partial struct PropertyToken
    {
        public static readonly System.Reflection.Emit.PropertyToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.PropertyToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.PropertyToken a, System.Reflection.Emit.PropertyToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.PropertyToken a, System.Reflection.Emit.PropertyToken b) { throw null; }
    }
    public sealed partial class SignatureHelper
    {
        internal SignatureHelper() { }
        public void AddArgument(System.Type clsArgument) { }
        public void AddArgument(System.Type argument, bool pinned) { }
        public void AddArgument(System.Type argument, System.Type[] requiredCustomModifiers, System.Type[] optionalCustomModifiers) { }
        public void AddArguments(System.Type[] arguments, System.Type[][] requiredCustomModifiers, System.Type[][] optionalCustomModifiers) { }
        public void AddSentinel() { }
        public override bool Equals(object obj) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetFieldSigHelper(System.Reflection.Module mod) { throw null; }
        public override int GetHashCode() { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetLocalVarSigHelper() { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetLocalVarSigHelper(System.Reflection.Module mod) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetMethodSigHelper(System.Runtime.InteropServices.CallingConvention unmanagedCallingConvention, System.Type returnType) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetMethodSigHelper(System.Reflection.CallingConventions callingConvention, System.Type returnType) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetMethodSigHelper(System.Reflection.Module mod, System.Runtime.InteropServices.CallingConvention unmanagedCallConv, System.Type returnType) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetMethodSigHelper(System.Reflection.Module mod, System.Reflection.CallingConventions callingConvention, System.Type returnType) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetMethodSigHelper(System.Reflection.Module mod, System.Type returnType, System.Type[] parameterTypes) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetPropertySigHelper(System.Reflection.Module mod, System.Reflection.CallingConventions callingConvention, System.Type returnType, System.Type[] requiredReturnTypeCustomModifiers, System.Type[] optionalReturnTypeCustomModifiers, System.Type[] parameterTypes, System.Type[][] requiredParameterTypeCustomModifiers, System.Type[][] optionalParameterTypeCustomModifiers) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetPropertySigHelper(System.Reflection.Module mod, System.Type returnType, System.Type[] parameterTypes) { throw null; }
        public static System.Reflection.Emit.SignatureHelper GetPropertySigHelper(System.Reflection.Module mod, System.Type returnType, System.Type[] requiredReturnTypeCustomModifiers, System.Type[] optionalReturnTypeCustomModifiers, System.Type[] parameterTypes, System.Type[][] requiredParameterTypeCustomModifiers, System.Type[][] optionalParameterTypeCustomModifiers) { throw null; }
        public byte[] GetSignature() { throw null; }
        public override string ToString() { throw null; }
    }
    public partial struct SignatureToken
    {
        public static readonly System.Reflection.Emit.SignatureToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.SignatureToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.SignatureToken a, System.Reflection.Emit.SignatureToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.SignatureToken a, System.Reflection.Emit.SignatureToken b) { throw null; }
    }
    public partial struct StringToken
    {
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.StringToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.StringToken a, System.Reflection.Emit.StringToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.StringToken a, System.Reflection.Emit.StringToken b) { throw null; }
    }
    public partial struct TypeToken
    {
        public static readonly System.Reflection.Emit.TypeToken Empty;
        private object _dummy;
        public int Token { get { throw null; } }
        public override int GetHashCode() { throw null; }
        public override bool Equals(object obj) { throw null; }
        public bool Equals(System.Reflection.Emit.TypeToken obj) { throw null; }
        public static bool operator ==(System.Reflection.Emit.TypeToken a, System.Reflection.Emit.TypeToken b) { throw null; }
        public static bool operator !=(System.Reflection.Emit.TypeToken a, System.Reflection.Emit.TypeToken b) { throw null; }
    }
}
