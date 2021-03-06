﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("SalesOrderHeaderSalesReason", Schema = "Sales")]
    public partial class SalesOrderHeaderSalesReason : INotifyPropertyChanged
    {
        #region Members
        private int _salesOrderId;
        [Key]
        public int SalesOrderId 
        { 
            get { return _salesOrderId; } 
            set { SetField(ref _salesOrderId, value, nameof(SalesOrderId)); } 
        }
        private int _salesReasonId;
        [Key]
        public int SalesReasonId 
        { 
            get { return _salesReasonId; } 
            set { SetField(ref _salesReasonId, value, nameof(SalesReasonId)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (SalesOrderId == default(int) && SalesReasonId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Sales].[SalesOrderHeaderSalesReason]
                (
                    [ModifiedDate],
                    [SalesOrderID],
                    [SalesReasonID]
                )
                VALUES
                (
                    @ModifiedDate,
                    @SalesOrderId,
                    @SalesReasonId
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Sales].[SalesOrderHeaderSalesReason] SET
                    [ModifiedDate] = @ModifiedDate,
                    [SalesOrderID] = @SalesOrderId,
                    [SalesReasonID] = @SalesReasonId
                WHERE
                    [SalesOrderID] = @SalesOrderId AND 
                    [SalesReasonID] = @SalesReasonId";
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
            SalesOrderHeaderSalesReason other = obj as SalesOrderHeaderSalesReason;
            if (other == null) return false;

            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (SalesOrderId != other.SalesOrderId)
                return false;
            if (SalesReasonId != other.SalesReasonId)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (SalesOrderId == default(int) ? 0 : SalesOrderId.GetHashCode());
                hash = hash * 23 + (SalesReasonId == default(int) ? 0 : SalesReasonId.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(SalesOrderHeaderSalesReason left, SalesOrderHeaderSalesReason right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SalesOrderHeaderSalesReason left, SalesOrderHeaderSalesReason right)
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
