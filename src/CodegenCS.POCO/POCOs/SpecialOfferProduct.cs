﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("SpecialOfferProduct", Schema = "Sales")]
    public partial class SpecialOfferProduct : INotifyPropertyChanged
    {
        #region Members
        private int _specialOfferId;
        [Key]
        public int SpecialOfferId 
        { 
            get { return _specialOfferId; } 
            set { SetField(ref _specialOfferId, value, nameof(SpecialOfferId)); } 
        }
        private int _productId;
        [Key]
        public int ProductId 
        { 
            get { return _productId; } 
            set { SetField(ref _productId, value, nameof(ProductId)); } 
        }
        private DateTime _modifiedDate;
        public DateTime ModifiedDate 
        { 
            get { return _modifiedDate; } 
            set { SetField(ref _modifiedDate, value, nameof(ModifiedDate)); } 
        }
        private Guid _rowguid;
        public Guid Rowguid 
        { 
            get { return _rowguid; } 
            set { SetField(ref _rowguid, value, nameof(Rowguid)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (SpecialOfferId == default(int) && ProductId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Sales].[SpecialOfferProduct]
                (
                    [ModifiedDate],
                    [ProductID],
                    [SpecialOfferID]
                )
                VALUES
                (
                    @ModifiedDate,
                    @ProductId,
                    @SpecialOfferId
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Sales].[SpecialOfferProduct] SET
                    [ModifiedDate] = @ModifiedDate,
                    [ProductID] = @ProductId,
                    [SpecialOfferID] = @SpecialOfferId
                WHERE
                    [SpecialOfferID] = @SpecialOfferId AND 
                    [ProductID] = @ProductId";
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
            SpecialOfferProduct other = obj as SpecialOfferProduct;
            if (other == null) return false;

            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (ProductId != other.ProductId)
                return false;
            if (Rowguid != other.Rowguid)
                return false;
            if (SpecialOfferId != other.SpecialOfferId)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (ProductId == default(int) ? 0 : ProductId.GetHashCode());
                hash = hash * 23 + (Rowguid == default(Guid) ? 0 : Rowguid.GetHashCode());
                hash = hash * 23 + (SpecialOfferId == default(int) ? 0 : SpecialOfferId.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(SpecialOfferProduct left, SpecialOfferProduct right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SpecialOfferProduct left, SpecialOfferProduct right)
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
