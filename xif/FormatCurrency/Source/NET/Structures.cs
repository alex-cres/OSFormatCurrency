using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using OutSystems.ObjectKeys;
using OutSystems.RuntimeCommon;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;

namespace OutSystems.NssFormatCurrency {

	/// <summary>
	/// Structure <code>STLocaleStructure</code> that represents the Service Studio structure
	///  <code>Locale</code> <p> Description: </p>
	/// </summary>
	[Serializable()]
	public partial struct STLocaleStructure: ISerializable, ITypedRecord<STLocaleStructure>, ISimpleRecord {
		internal static readonly GlobalObjectKey IdName = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*mFsB6TdYmUGCI0Zi1SP+5g");
		internal static readonly GlobalObjectKey IdRFC4646 = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*XTOLkVZ6X0Kg2_x76WU9ag");
		internal static readonly GlobalObjectKey IdCurrencyDecimalDigits = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*l0QOYLNBT022t08wHpXfuw");
		internal static readonly GlobalObjectKey IdCurrencyDecimalSeparator = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*O2gqYmIguUuy4YBjPWnxRQ");
		internal static readonly GlobalObjectKey IdCurrencyGroupSeparator = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*6JGqU7Un4kenrytV1GVr_A");
		internal static readonly GlobalObjectKey IdCurrencyGroupSizes = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*ooNFgA8I4kuXT3fgPaFerw");
		internal static readonly GlobalObjectKey IdCurrencyNegativePattern = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*TmbS0NzXz0+PBuxmBeu3zw");
		internal static readonly GlobalObjectKey IdCurrencyPositivePattern = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*HSnYiAkls0KRqUZrLzl+jQ");
		internal static readonly GlobalObjectKey IdNegativeSign = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*rtclubB18EyfVoqsnIFM7Q");
		internal static readonly GlobalObjectKey IdCurrencySymbol = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*MwM2QehuT0WnhU+oi4Cq3A");
		internal static readonly GlobalObjectKey IdNativeDigits = GlobalObjectKey.Parse("lRL20dNzOEqLByTct7MrXA*5UARhH06qEyT6reDE_Q1ZQ");

		public static void EnsureInitialized() {}
		[System.Xml.Serialization.XmlElement("Name")]
		public string ssName;

		[System.Xml.Serialization.XmlElement("RFC4646")]
		public string ssRFC4646;

		[System.Xml.Serialization.XmlElement("CurrencyDecimalDigits")]
		public string ssCurrencyDecimalDigits;

		[System.Xml.Serialization.XmlElement("CurrencyDecimalSeparator")]
		public string ssCurrencyDecimalSeparator;

		[System.Xml.Serialization.XmlElement("CurrencyGroupSeparator")]
		public string ssCurrencyGroupSeparator;

		[System.Xml.Serialization.XmlElement("CurrencyGroupSizes")]
		public string ssCurrencyGroupSizes;

		[System.Xml.Serialization.XmlElement("CurrencyNegativePattern")]
		public string ssCurrencyNegativePattern;

		[System.Xml.Serialization.XmlElement("CurrencyPositivePattern")]
		public string ssCurrencyPositivePattern;

		[System.Xml.Serialization.XmlElement("NegativeSign")]
		public string ssNegativeSign;

		[System.Xml.Serialization.XmlElement("CurrencySymbol")]
		public string ssCurrencySymbol;

		[System.Xml.Serialization.XmlElement("NativeDigits")]
		public string ssNativeDigits;


		public BitArray OptimizedAttributes;

		public STLocaleStructure(params string[] dummy) {
			OptimizedAttributes = null;
			ssName = "";
			ssRFC4646 = "";
			ssCurrencyDecimalDigits = "";
			ssCurrencyDecimalSeparator = "";
			ssCurrencyGroupSeparator = "";
			ssCurrencyGroupSizes = "";
			ssCurrencyNegativePattern = "";
			ssCurrencyPositivePattern = "";
			ssNegativeSign = "";
			ssCurrencySymbol = "";
			ssNativeDigits = "";
		}

		public BitArray[] GetDefaultOptimizedValues() {
			BitArray[] all = new BitArray[0];
			return all;
		}

		public BitArray[] AllOptimizedAttributes {
			set {
				if (value == null) {
				} else {
				}
			}
			get {
				BitArray[] all = new BitArray[0];
				return all;
			}
		}

		/// <summary>
		/// Read a record from database
		/// </summary>
		/// <param name="r"> Data base reader</param>
		/// <param name="index"> index</param>
		public void Read(IDataReader r, ref int index) {
			ssName = r.ReadText(index++, "Locale.Name", "");
			ssRFC4646 = r.ReadText(index++, "Locale.RFC4646", "");
			ssCurrencyDecimalDigits = r.ReadText(index++, "Locale.CurrencyDecimalDigits", "");
			ssCurrencyDecimalSeparator = r.ReadText(index++, "Locale.CurrencyDecimalSeparator", "");
			ssCurrencyGroupSeparator = r.ReadText(index++, "Locale.CurrencyGroupSeparator", "");
			ssCurrencyGroupSizes = r.ReadText(index++, "Locale.CurrencyGroupSizes", "");
			ssCurrencyNegativePattern = r.ReadText(index++, "Locale.CurrencyNegativePattern", "");
			ssCurrencyPositivePattern = r.ReadText(index++, "Locale.CurrencyPositivePattern", "");
			ssNegativeSign = r.ReadText(index++, "Locale.NegativeSign", "");
			ssCurrencySymbol = r.ReadText(index++, "Locale.CurrencySymbol", "");
			ssNativeDigits = r.ReadText(index++, "Locale.NativeDigits", "");
		}
		/// <summary>
		/// Read from database
		/// </summary>
		/// <param name="r"> Data reader</param>
		public void ReadDB(IDataReader r) {
			int index = 0;
			Read(r, ref index);
		}

		/// <summary>
		/// Read from record
		/// </summary>
		/// <param name="r"> Record</param>
		public void ReadIM(STLocaleStructure r) {
			this = r;
		}


		public static bool operator == (STLocaleStructure a, STLocaleStructure b) {
			if (a.ssName != b.ssName) return false;
			if (a.ssRFC4646 != b.ssRFC4646) return false;
			if (a.ssCurrencyDecimalDigits != b.ssCurrencyDecimalDigits) return false;
			if (a.ssCurrencyDecimalSeparator != b.ssCurrencyDecimalSeparator) return false;
			if (a.ssCurrencyGroupSeparator != b.ssCurrencyGroupSeparator) return false;
			if (a.ssCurrencyGroupSizes != b.ssCurrencyGroupSizes) return false;
			if (a.ssCurrencyNegativePattern != b.ssCurrencyNegativePattern) return false;
			if (a.ssCurrencyPositivePattern != b.ssCurrencyPositivePattern) return false;
			if (a.ssNegativeSign != b.ssNegativeSign) return false;
			if (a.ssCurrencySymbol != b.ssCurrencySymbol) return false;
			if (a.ssNativeDigits != b.ssNativeDigits) return false;
			return true;
		}

		public static bool operator != (STLocaleStructure a, STLocaleStructure b) {
			return !(a==b);
		}

		public override bool Equals(object o) {
			if (o.GetType() != typeof(STLocaleStructure)) return false;
			return (this == (STLocaleStructure) o);
		}

		public override int GetHashCode() {
			try {
				return base.GetHashCode()
				^ ssName.GetHashCode()
				^ ssRFC4646.GetHashCode()
				^ ssCurrencyDecimalDigits.GetHashCode()
				^ ssCurrencyDecimalSeparator.GetHashCode()
				^ ssCurrencyGroupSeparator.GetHashCode()
				^ ssCurrencyGroupSizes.GetHashCode()
				^ ssCurrencyNegativePattern.GetHashCode()
				^ ssCurrencyPositivePattern.GetHashCode()
				^ ssNegativeSign.GetHashCode()
				^ ssCurrencySymbol.GetHashCode()
				^ ssNativeDigits.GetHashCode()
				;
			} catch {
				return base.GetHashCode();
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context) {
			Type objInfo = this.GetType();
			FieldInfo[] fields;
			fields = objInfo.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			for (int i = 0; i < fields.Length; i++)
			if (fields[i] .FieldType.IsSerializable)
			info.AddValue(fields[i] .Name, fields[i] .GetValue(this));
		}

		public STLocaleStructure(SerializationInfo info, StreamingContext context) {
			OptimizedAttributes = null;
			ssName = "";
			ssRFC4646 = "";
			ssCurrencyDecimalDigits = "";
			ssCurrencyDecimalSeparator = "";
			ssCurrencyGroupSeparator = "";
			ssCurrencyGroupSizes = "";
			ssCurrencyNegativePattern = "";
			ssCurrencyPositivePattern = "";
			ssNegativeSign = "";
			ssCurrencySymbol = "";
			ssNativeDigits = "";
			Type objInfo = this.GetType();
			FieldInfo fieldInfo = null;
			fieldInfo = objInfo.GetField("ssName", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssName' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssName = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssRFC4646", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssRFC4646' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssRFC4646 = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyDecimalDigits", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyDecimalDigits' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyDecimalDigits = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyDecimalSeparator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyDecimalSeparator' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyDecimalSeparator = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyGroupSeparator", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyGroupSeparator' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyGroupSeparator = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyGroupSizes", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyGroupSizes' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyGroupSizes = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyNegativePattern", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyNegativePattern' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyNegativePattern = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencyPositivePattern", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencyPositivePattern' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencyPositivePattern = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssNegativeSign", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssNegativeSign' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssNegativeSign = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssCurrencySymbol", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssCurrencySymbol' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssCurrencySymbol = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
			fieldInfo = objInfo.GetField("ssNativeDigits", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssNativeDigits' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssNativeDigits = (string) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
		}

		public void RecursiveReset() {
		}

		public void InternalRecursiveSave() {
		}


		public STLocaleStructure Duplicate() {
			STLocaleStructure t;
			t.ssName = this.ssName;
			t.ssRFC4646 = this.ssRFC4646;
			t.ssCurrencyDecimalDigits = this.ssCurrencyDecimalDigits;
			t.ssCurrencyDecimalSeparator = this.ssCurrencyDecimalSeparator;
			t.ssCurrencyGroupSeparator = this.ssCurrencyGroupSeparator;
			t.ssCurrencyGroupSizes = this.ssCurrencyGroupSizes;
			t.ssCurrencyNegativePattern = this.ssCurrencyNegativePattern;
			t.ssCurrencyPositivePattern = this.ssCurrencyPositivePattern;
			t.ssNegativeSign = this.ssNegativeSign;
			t.ssCurrencySymbol = this.ssCurrencySymbol;
			t.ssNativeDigits = this.ssNativeDigits;
			t.OptimizedAttributes = null;
			return t;
		}

		IRecord IRecord.Duplicate() {
			return Duplicate();
		}

		public void ToXml(Object parent, System.Xml.XmlElement baseElem, String fieldName, int detailLevel) {
			System.Xml.XmlElement recordElem = VarValue.AppendChild(baseElem, "Structure");
			if (fieldName != null) {
				VarValue.AppendAttribute(recordElem, "debug.field", fieldName);
				fieldName = fieldName.ToLowerInvariant();
			}
			if (detailLevel > 0) {
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".Name")) VarValue.AppendAttribute(recordElem, "Name", ssName, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "Name");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".RFC4646")) VarValue.AppendAttribute(recordElem, "RFC4646", ssRFC4646, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "RFC4646");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyDecimalDigits")) VarValue.AppendAttribute(recordElem, "CurrencyDecimalDigits", ssCurrencyDecimalDigits, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyDecimalDigits");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyDecimalSeparator")) VarValue.AppendAttribute(recordElem, "CurrencyDecimalSeparator", ssCurrencyDecimalSeparator, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyDecimalSeparator");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyGroupSeparator")) VarValue.AppendAttribute(recordElem, "CurrencyGroupSeparator", ssCurrencyGroupSeparator, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyGroupSeparator");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyGroupSizes")) VarValue.AppendAttribute(recordElem, "CurrencyGroupSizes", ssCurrencyGroupSizes, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyGroupSizes");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyNegativePattern")) VarValue.AppendAttribute(recordElem, "CurrencyNegativePattern", ssCurrencyNegativePattern, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyNegativePattern");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencyPositivePattern")) VarValue.AppendAttribute(recordElem, "CurrencyPositivePattern", ssCurrencyPositivePattern, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencyPositivePattern");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".NegativeSign")) VarValue.AppendAttribute(recordElem, "NegativeSign", ssNegativeSign, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "NegativeSign");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".CurrencySymbol")) VarValue.AppendAttribute(recordElem, "CurrencySymbol", ssCurrencySymbol, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "CurrencySymbol");
				if (!VarValue.FieldIsOptimized(parent, fieldName + ".NativeDigits")) VarValue.AppendAttribute(recordElem, "NativeDigits", ssNativeDigits, detailLevel, TypeKind.Text); else VarValue.AppendOptimizedAttribute(recordElem, "NativeDigits");
			} else {
				VarValue.AppendDeferredEvaluationElement(recordElem);
			}
		}

		public void EvaluateFields(VarValue variable, Object parent, String baseName, String fields) {
			String head = VarValue.GetHead(fields);
			String tail = VarValue.GetTail(fields);
			variable.Found = false;
			if (head == "name") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".Name")) variable.Value = ssName; else variable.Optimized = true;
			} else if (head == "rfc4646") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".RFC4646")) variable.Value = ssRFC4646; else variable.Optimized = true;
			} else if (head == "currencydecimaldigits") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyDecimalDigits")) variable.Value = ssCurrencyDecimalDigits; else variable.Optimized = true;
			} else if (head == "currencydecimalseparator") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyDecimalSeparator")) variable.Value = ssCurrencyDecimalSeparator; else variable.Optimized = true;
			} else if (head == "currencygroupseparator") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyGroupSeparator")) variable.Value = ssCurrencyGroupSeparator; else variable.Optimized = true;
			} else if (head == "currencygroupsizes") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyGroupSizes")) variable.Value = ssCurrencyGroupSizes; else variable.Optimized = true;
			} else if (head == "currencynegativepattern") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyNegativePattern")) variable.Value = ssCurrencyNegativePattern; else variable.Optimized = true;
			} else if (head == "currencypositivepattern") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencyPositivePattern")) variable.Value = ssCurrencyPositivePattern; else variable.Optimized = true;
			} else if (head == "negativesign") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".NegativeSign")) variable.Value = ssNegativeSign; else variable.Optimized = true;
			} else if (head == "currencysymbol") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".CurrencySymbol")) variable.Value = ssCurrencySymbol; else variable.Optimized = true;
			} else if (head == "nativedigits") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".NativeDigits")) variable.Value = ssNativeDigits; else variable.Optimized = true;
			}
			if (variable.Found && tail != null) variable.EvaluateFields(this, head, tail);
		}

		public bool ChangedAttributeGet(GlobalObjectKey key) {
			throw new Exception("Method not Supported");
		}

		public bool OptimizedAttributeGet(GlobalObjectKey key) {
			throw new Exception("Method not Supported");
		}

		public object AttributeGet(GlobalObjectKey key) {
			if (key == IdName) {
				return ssName;
			} else if (key == IdRFC4646) {
				return ssRFC4646;
			} else if (key == IdCurrencyDecimalDigits) {
				return ssCurrencyDecimalDigits;
			} else if (key == IdCurrencyDecimalSeparator) {
				return ssCurrencyDecimalSeparator;
			} else if (key == IdCurrencyGroupSeparator) {
				return ssCurrencyGroupSeparator;
			} else if (key == IdCurrencyGroupSizes) {
				return ssCurrencyGroupSizes;
			} else if (key == IdCurrencyNegativePattern) {
				return ssCurrencyNegativePattern;
			} else if (key == IdCurrencyPositivePattern) {
				return ssCurrencyPositivePattern;
			} else if (key == IdNegativeSign) {
				return ssNegativeSign;
			} else if (key == IdCurrencySymbol) {
				return ssCurrencySymbol;
			} else if (key == IdNativeDigits) {
				return ssNativeDigits;
			} else {
				throw new Exception("Invalid key");
			}
		}
		public void FillFromOther(IRecord other) {
			if (other == null) return;
			ssName = (string) other.AttributeGet(IdName);
			ssRFC4646 = (string) other.AttributeGet(IdRFC4646);
			ssCurrencyDecimalDigits = (string) other.AttributeGet(IdCurrencyDecimalDigits);
			ssCurrencyDecimalSeparator = (string) other.AttributeGet(IdCurrencyDecimalSeparator);
			ssCurrencyGroupSeparator = (string) other.AttributeGet(IdCurrencyGroupSeparator);
			ssCurrencyGroupSizes = (string) other.AttributeGet(IdCurrencyGroupSizes);
			ssCurrencyNegativePattern = (string) other.AttributeGet(IdCurrencyNegativePattern);
			ssCurrencyPositivePattern = (string) other.AttributeGet(IdCurrencyPositivePattern);
			ssNegativeSign = (string) other.AttributeGet(IdNegativeSign);
			ssCurrencySymbol = (string) other.AttributeGet(IdCurrencySymbol);
			ssNativeDigits = (string) other.AttributeGet(IdNativeDigits);
		}
		public bool IsDefault() {
			STLocaleStructure defaultStruct = new STLocaleStructure(null);
			if (this.ssName != defaultStruct.ssName) return false;
			if (this.ssRFC4646 != defaultStruct.ssRFC4646) return false;
			if (this.ssCurrencyDecimalDigits != defaultStruct.ssCurrencyDecimalDigits) return false;
			if (this.ssCurrencyDecimalSeparator != defaultStruct.ssCurrencyDecimalSeparator) return false;
			if (this.ssCurrencyGroupSeparator != defaultStruct.ssCurrencyGroupSeparator) return false;
			if (this.ssCurrencyGroupSizes != defaultStruct.ssCurrencyGroupSizes) return false;
			if (this.ssCurrencyNegativePattern != defaultStruct.ssCurrencyNegativePattern) return false;
			if (this.ssCurrencyPositivePattern != defaultStruct.ssCurrencyPositivePattern) return false;
			if (this.ssNegativeSign != defaultStruct.ssNegativeSign) return false;
			if (this.ssCurrencySymbol != defaultStruct.ssCurrencySymbol) return false;
			if (this.ssNativeDigits != defaultStruct.ssNativeDigits) return false;
			return true;
		}
	} // STLocaleStructure

} // OutSystems.NssFormatCurrency
