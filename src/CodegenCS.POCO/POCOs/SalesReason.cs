﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("SalesReason", Schema = "Sales")]
    public partial class SalesReason : INotifyPropertyChanged
    {
        #region Members
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
        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { SetField(ref _name, value, nameof(Name)); } 
        }
        private string _reasonType;
        public string ReasonType 
        { 
            get { return _reasonType; } 
            set { SetField(ref _reasonType, value, nameof(ReasonType)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (SalesReasonId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Sales].[SalesReason]
                (
                    [ModifiedDate],
                    [Name],
                    [ReasonType]
                )
                VALUES
                (
                    @ModifiedDate,
                    @Name,
                    @ReasonType
                )";

                this.SalesReasonId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Sales].[SalesReason] SET
                    [ModifiedDate] = @ModifiedDate,
                    [Name] = @Name,
                    [ReasonType] = @ReasonType
                WHERE
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
            SalesReason other = obj as SalesReason;
            if (other == null) return false;

            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Name != other.Name)
                return false;
            if (ReasonType != other.ReasonType)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                hash = hash * 23 + (ReasonType == null ? 0 : ReasonType.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(SalesReason left, SalesReason right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SalesReason left, SalesReason right)
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
