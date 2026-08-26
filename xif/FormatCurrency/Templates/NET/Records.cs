using System;
using System.Collections;
using System.Data;
using System.Runtime.Serialization;
using System.Reflection;
using System.Xml;
using OutSystems.ObjectKeys;
using OutSystems.RuntimeCommon;
using OutSystems.HubEdition.RuntimePlatform;
using OutSystems.HubEdition.RuntimePlatform.Db;
using OutSystems.Internal.Db;

namespace OutSystems.NssFormatCurrency {

	/// <summary>
	/// Structure <code>RCLocaleRecord</code>
	/// </summary>
	[Serializable()]
	public partial struct RCLocaleRecord: ISerializable, ITypedRecord<RCLocaleRecord> {
		internal static readonly GlobalObjectKey IdLocale = GlobalObjectKey.Parse("2UmDmepsh0WSfJ_D1JexCA*5YUxmUgin3_NN5WzYT2c9g");

		public static void EnsureInitialized() {}
		[System.Xml.Serialization.XmlElement("Locale")]
		public STLocaleStructure ssSTLocale;


		public static implicit operator STLocaleStructure(RCLocaleRecord r) {
			return r.ssSTLocale;
		}

		public static implicit operator RCLocaleRecord(STLocaleStructure r) {
			RCLocaleRecord res = new RCLocaleRecord(null);
			res.ssSTLocale = r;
			return res;
		}

		public BitArray OptimizedAttributes;

		public RCLocaleRecord(params string[] dummy) {
			OptimizedAttributes = null;
			ssSTLocale = new STLocaleStructure(null);
		}

		public BitArray[] GetDefaultOptimizedValues() {
			BitArray[] all = new BitArray[1];
			all[0] = null;
			return all;
		}

		public BitArray[] AllOptimizedAttributes {
			set {
				if (value == null) {
				} else {
					ssSTLocale.OptimizedAttributes = value[0];
				}
			}
			get {
				BitArray[] all = new BitArray[1];
				all[0] = null;
				return all;
			}
		}

		/// <summary>
		/// Read a record from database
		/// </summary>
		/// <param name="r"> Data base reader</param>
		/// <param name="index"> index</param>
		public void Read(IDataReader r, ref int index) {
			ssSTLocale.Read(r, ref index);
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
		public void ReadIM(RCLocaleRecord r) {
			this = r;
		}


		public static bool operator == (RCLocaleRecord a, RCLocaleRecord b) {
			if (a.ssSTLocale != b.ssSTLocale) return false;
			return true;
		}

		public static bool operator != (RCLocaleRecord a, RCLocaleRecord b) {
			return !(a==b);
		}

		public override bool Equals(object o) {
			if (o.GetType() != typeof(RCLocaleRecord)) return false;
			return (this == (RCLocaleRecord) o);
		}

		public override int GetHashCode() {
			try {
				return base.GetHashCode()
				^ ssSTLocale.GetHashCode()
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

		public RCLocaleRecord(SerializationInfo info, StreamingContext context) {
			OptimizedAttributes = null;
			ssSTLocale = new STLocaleStructure(null);
			Type objInfo = this.GetType();
			FieldInfo fieldInfo = null;
			fieldInfo = objInfo.GetField("ssSTLocale", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if (fieldInfo == null) {
				throw new Exception("The field named 'ssSTLocale' was not found.");
			}
			if (fieldInfo.FieldType.IsSerializable) {
				ssSTLocale = (STLocaleStructure) info.GetValue(fieldInfo.Name, fieldInfo.FieldType);
			}
		}

		public void RecursiveReset() {
			ssSTLocale.RecursiveReset();
		}

		public void InternalRecursiveSave() {
			ssSTLocale.InternalRecursiveSave();
		}


		public RCLocaleRecord Duplicate() {
			RCLocaleRecord t;
			t.ssSTLocale = (STLocaleStructure) this.ssSTLocale.Duplicate();
			t.OptimizedAttributes = null;
			return t;
		}

		IRecord IRecord.Duplicate() {
			return Duplicate();
		}

		public void ToXml(Object parent, System.Xml.XmlElement baseElem, String fieldName, int detailLevel) {
			System.Xml.XmlElement recordElem = VarValue.AppendChild(baseElem, "Record");
			if (fieldName != null) {
				VarValue.AppendAttribute(recordElem, "debug.field", fieldName);
			}
			if (detailLevel > 0) {
				ssSTLocale.ToXml(this, recordElem, "Locale", detailLevel - 1);
			} else {
				VarValue.AppendDeferredEvaluationElement(recordElem);
			}
		}

		public void EvaluateFields(VarValue variable, Object parent, String baseName, String fields) {
			String head = VarValue.GetHead(fields);
			String tail = VarValue.GetTail(fields);
			variable.Found = false;
			if (head == "locale") {
				if (!VarValue.FieldIsOptimized(parent, baseName + ".Locale")) variable.Value = ssSTLocale; else variable.Optimized = true;
				variable.SetFieldName("locale");
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
			if (key == IdLocale) {
				return ssSTLocale;
			} else {
				throw new Exception("Invalid key");
			}
		}
		public void FillFromOther(IRecord other) {
			if (other == null) return;
			ssSTLocale.FillFromOther((IRecord) other.AttributeGet(IdLocale));
		}
		public bool IsDefault() {
			RCLocaleRecord defaultStruct = new RCLocaleRecord(null);
			if (this.ssSTLocale != defaultStruct.ssSTLocale) return false;
			return true;
		}
	} // RCLocaleRecord
}
