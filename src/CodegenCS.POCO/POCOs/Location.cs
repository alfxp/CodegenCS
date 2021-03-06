﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("Location", Schema = "Production")]
    public partial class Location : INotifyPropertyChanged
    {
        #region Members
        private short _locationId;
        [Key]
        public short LocationId 
        { 
            get { return _locationId; } 
            set { SetField(ref _locationId, value, nameof(LocationId)); } 
        }
        private decimal _availability;
        public decimal Availability 
        { 
            get { return _availability; } 
            set { SetField(ref _availability, value, nameof(Availability)); } 
        }
        private decimal _costRate;
        public decimal CostRate 
        { 
            get { return _costRate; } 
            set { SetField(ref _costRate, value, nameof(CostRate)); } 
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
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (LocationId == default(short))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Production].[Location]
                (
                    [Availability],
                    [CostRate],
                    [ModifiedDate],
                    [Name]
                )
                VALUES
                (
                    @Availability,
                    @CostRate,
                    @ModifiedDate,
                    @Name
                )";

                this.LocationId = conn.Query<short>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Production].[Location] SET
                    [Availability] = @Availability,
                    [CostRate] = @CostRate,
                    [ModifiedDate] = @ModifiedDate,
                    [Name] = @Name
                WHERE
                    [LocationID] = @LocationId";
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
            Location other = obj as Location;
            if (other == null) return false;

            if (Availability != other.Availability)
                return false;
            if (CostRate != other.CostRate)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Name != other.Name)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Availability == default(decimal) ? 0 : Availability.GetHashCode());
                hash = hash * 23 + (CostRate == default(decimal) ? 0 : CostRate.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(Location left, Location right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Location left, Location right)
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
