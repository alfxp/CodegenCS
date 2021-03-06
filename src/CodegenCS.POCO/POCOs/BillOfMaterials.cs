﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("BillOfMaterials", Schema = "Production")]
    public partial class BillOfMaterials : INotifyPropertyChanged
    {
        #region Members
        private int _billOfMaterialsId;
        [Key]
        public int BillOfMaterialsId 
        { 
            get { return _billOfMaterialsId; } 
            set { SetField(ref _billOfMaterialsId, value, nameof(BillOfMaterialsId)); } 
        }
        private short _bomLevel;
        public short BomLevel 
        { 
            get { return _bomLevel; } 
            set { SetField(ref _bomLevel, value, nameof(BomLevel)); } 
        }
        private int _componentId;
        public int ComponentId 
        { 
            get { return _componentId; } 
            set { SetField(ref _componentId, value, nameof(ComponentId)); } 
        }
        private DateTime? _endDate;
        public DateTime? EndDate 
        { 
            get { return _endDate; } 
            set { SetField(ref _endDate, value, nameof(EndDate)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private decimal _perAssemblyQty;
        public decimal PerAssemblyQty 
        { 
            get { return _perAssemblyQty; } 
            set { SetField(ref _perAssemblyQty, value, nameof(PerAssemblyQty)); } 
        }
        private int? _productAssemblyId;
        public int? ProductAssemblyId 
        { 
            get { return _productAssemblyId; } 
            set { SetField(ref _productAssemblyId, value, nameof(ProductAssemblyId)); } 
        }
        private DateTime _startDate;
        public DateTime StartDate 
        { 
            get { return _startDate; } 
            set { SetField(ref _startDate, value, nameof(StartDate)); } 
        }
        private string _unitMeasureCode;
        public string UnitMeasureCode 
        { 
            get { return _unitMeasureCode; } 
            set { SetField(ref _unitMeasureCode, value, nameof(UnitMeasureCode)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (BillOfMaterialsId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Production].[BillOfMaterials]
                (
                    [BOMLevel],
                    [ComponentID],
                    [EndDate],
                    [ModifiedDate],
                    [PerAssemblyQty],
                    [ProductAssemblyID],
                    [StartDate],
                    [UnitMeasureCode]
                )
                VALUES
                (
                    @BomLevel,
                    @ComponentId,
                    @EndDate,
                    @ModifiedDate,
                    @PerAssemblyQty,
                    @ProductAssemblyId,
                    @StartDate,
                    @UnitMeasureCode
                )";

                this.BillOfMaterialsId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Production].[BillOfMaterials] SET
                    [BOMLevel] = @BomLevel,
                    [ComponentID] = @ComponentId,
                    [EndDate] = @EndDate,
                    [ModifiedDate] = @ModifiedDate,
                    [PerAssemblyQty] = @PerAssemblyQty,
                    [ProductAssemblyID] = @ProductAssemblyId,
                    [StartDate] = @StartDate,
                    [UnitMeasureCode] = @UnitMeasureCode
                WHERE
                    [BillOfMaterialsID] = @BillOfMaterialsId";
                conn.Execute(cmd, this);
            }
        }
        #endregion ActiveRecord

        #region Equals/GetHashCode
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            BillOfMaterials other = obj as BillOfMaterials;
            if (other == null) return false;

            if (BomLevel != other.BomLevel)
                return false;
            if (ComponentId != other.ComponentId)
                return false;
            if (EndDate != other.EndDate)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (PerAssemblyQty != other.PerAssemblyQty)
                return false;
            if (ProductAssemblyId != other.ProductAssemblyId)
                return false;
            if (StartDate != other.StartDate)
                return false;
            if (UnitMeasureCode != other.UnitMeasureCode)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (BomLevel == default(short) ? 0 : BomLevel.GetHashCode());
                hash = hash * 23 + (ComponentId == default(int) ? 0 : ComponentId.GetHashCode());
                hash = hash * 23 + (EndDate == null ? 0 : EndDate.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (PerAssemblyQty == default(decimal) ? 0 : PerAssemblyQty.GetHashCode());
                hash = hash * 23 + (ProductAssemblyId == null ? 0 : ProductAssemblyId.GetHashCode());
                hash = hash * 23 + (StartDate == default(DateTime) ? 0 : StartDate.GetHashCode());
                hash = hash * 23 + (UnitMeasureCode == null ? 0 : UnitMeasureCode.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(BillOfMaterials left, BillOfMaterials right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BillOfMaterials left, BillOfMaterials right)
        {
            return !Equals(left, right);
        }

        #endregion Equals/GetHashCode

        #region INotifyPropertyChanged/IsDirty
        public HashSet<string> ChangedProperties = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public void MarkAsClean()
        {
            ChangedProperties.Clear();
        }
        public virtual bool IsDirty => ChangedProperties.Any();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetField<T>(ref T field, T value, string propertyName) {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                ChangedProperties.Add(propertyName);
                OnPropertyChanged(propertyName);
            }
        }
        protected virtual void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion INotifyPropertyChanged/IsDirty
    }
}
