﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("EmployeeDepartmentHistory", Schema = "HumanResources")]
    public partial class EmployeeDepartmentHistory : INotifyPropertyChanged
    {
        #region Members
        private int _businessEntityId;
        [Key]
        public int BusinessEntityId 
        { 
            get { return _businessEntityId; } 
            set { SetField(ref _businessEntityId, value, nameof(BusinessEntityId)); } 
        }
        private short _departmentId;
        [Key]
        public short DepartmentId 
        { 
            get { return _departmentId; } 
            set { SetField(ref _departmentId, value, nameof(DepartmentId)); } 
        }
        private byte _shiftId;
        [Key]
        public byte ShiftId 
        { 
            get { return _shiftId; } 
            set { SetField(ref _shiftId, value, nameof(ShiftId)); } 
        }
        private DateTime _startDate;
        [Key]
        public DateTime StartDate 
        { 
            get { return _startDate; } 
            set { SetField(ref _startDate, value, nameof(StartDate)); } 
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
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (BusinessEntityId == default(int) && DepartmentId == default(short) && ShiftId == default(byte) && StartDate == default(DateTime))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [HumanResources].[EmployeeDepartmentHistory]
                (
                    [BusinessEntityID],
                    [DepartmentID],
                    [EndDate],
                    [ModifiedDate],
                    [ShiftID],
                    [StartDate]
                )
                VALUES
                (
                    @BusinessEntityId,
                    @DepartmentId,
                    @EndDate,
                    @ModifiedDate,
                    @ShiftId,
                    @StartDate
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [HumanResources].[EmployeeDepartmentHistory] SET
                    [BusinessEntityID] = @BusinessEntityId,
                    [DepartmentID] = @DepartmentId,
                    [EndDate] = @EndDate,
                    [ModifiedDate] = @ModifiedDate,
                    [ShiftID] = @ShiftId,
                    [StartDate] = @StartDate
                WHERE
                    [BusinessEntityID] = @BusinessEntityId AND 
                    [DepartmentID] = @DepartmentId AND 
                    [ShiftID] = @ShiftId AND 
                    [StartDate] = @StartDate";
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
            EmployeeDepartmentHistory other = obj as EmployeeDepartmentHistory;
            if (other == null) return false;

            if (BusinessEntityId != other.BusinessEntityId)
                return false;
            if (DepartmentId != other.DepartmentId)
                return false;
            if (EndDate != other.EndDate)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (ShiftId != other.ShiftId)
                return false;
            if (StartDate != other.StartDate)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (BusinessEntityId == default(int) ? 0 : BusinessEntityId.GetHashCode());
                hash = hash * 23 + (DepartmentId == default(short) ? 0 : DepartmentId.GetHashCode());
                hash = hash * 23 + (EndDate == null ? 0 : EndDate.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (ShiftId == default(byte) ? 0 : ShiftId.GetHashCode());
                hash = hash * 23 + (StartDate == default(DateTime) ? 0 : StartDate.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(EmployeeDepartmentHistory left, EmployeeDepartmentHistory right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EmployeeDepartmentHistory left, EmployeeDepartmentHistory right)
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
