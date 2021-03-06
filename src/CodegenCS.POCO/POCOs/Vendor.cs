﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    [Table("Vendor", Schema = "Purchasing")]
    public partial class Vendor : INotifyPropertyChanged
    {
        #region Members
        private int _businessEntityId;
        [Key]
        public int BusinessEntityId 
        { 
            get { return _businessEntityId; } 
            set { SetField(ref _businessEntityId, value, nameof(BusinessEntityId)); } 
        }
        private string _accountNumber;
        public string AccountNumber 
        { 
            get { return _accountNumber; } 
            set { SetField(ref _accountNumber, value, nameof(AccountNumber)); } 
        }
        private bool _activeFlag;
        public bool ActiveFlag 
        { 
            get { return _activeFlag; } 
            set { SetField(ref _activeFlag, value, nameof(ActiveFlag)); } 
        }
        private byte _creditRating;
        public byte CreditRating 
        { 
            get { return _creditRating; } 
            set { SetField(ref _creditRating, value, nameof(CreditRating)); } 
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
        private bool _preferredVendorStatus;
        public bool PreferredVendorStatus 
        { 
            get { return _preferredVendorStatus; } 
            set { SetField(ref _preferredVendorStatus, value, nameof(PreferredVendorStatus)); } 
        }
        private string _purchasingWebServiceUrl;
        public string PurchasingWebServiceUrl 
        { 
            get { return _purchasingWebServiceUrl; } 
            set { SetField(ref _purchasingWebServiceUrl, value, nameof(PurchasingWebServiceUrl)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (BusinessEntityId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [Purchasing].[Vendor]
                (
                    [AccountNumber],
                    [ActiveFlag],
                    [BusinessEntityID],
                    [CreditRating],
                    [ModifiedDate],
                    [Name],
                    [PreferredVendorStatus],
                    [PurchasingWebServiceURL]
                )
                VALUES
                (
                    @AccountNumber,
                    @ActiveFlag,
                    @BusinessEntityId,
                    @CreditRating,
                    @ModifiedDate,
                    @Name,
                    @PreferredVendorStatus,
                    @PurchasingWebServiceUrl
                )";

                conn.Execute(cmd, this);
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [Purchasing].[Vendor] SET
                    [AccountNumber] = @AccountNumber,
                    [ActiveFlag] = @ActiveFlag,
                    [BusinessEntityID] = @BusinessEntityId,
                    [CreditRating] = @CreditRating,
                    [ModifiedDate] = @ModifiedDate,
                    [Name] = @Name,
                    [PreferredVendorStatus] = @PreferredVendorStatus,
                    [PurchasingWebServiceURL] = @PurchasingWebServiceUrl
                WHERE
                    [BusinessEntityID] = @BusinessEntityId";
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
            Vendor other = obj as Vendor;
            if (other == null) return false;

            if (AccountNumber != other.AccountNumber)
                return false;
            if (ActiveFlag != other.ActiveFlag)
                return false;
            if (BusinessEntityId != other.BusinessEntityId)
                return false;
            if (CreditRating != other.CreditRating)
                return false;
            if (ModifiedDate != other.ModifiedDate)
                return false;
            if (Name != other.Name)
                return false;
            if (PreferredVendorStatus != other.PreferredVendorStatus)
                return false;
            if (PurchasingWebServiceUrl != other.PurchasingWebServiceUrl)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (AccountNumber == null ? 0 : AccountNumber.GetHashCode());
                hash = hash * 23 + (ActiveFlag == default(bool) ? 0 : ActiveFlag.GetHashCode());
                hash = hash * 23 + (BusinessEntityId == default(int) ? 0 : BusinessEntityId.GetHashCode());
                hash = hash * 23 + (CreditRating == default(byte) ? 0 : CreditRating.GetHashCode());
                hash = hash * 23 + (ModifiedDate == default(DateTime) ? 0 : ModifiedDate.GetHashCode());
                hash = hash * 23 + (Name == null ? 0 : Name.GetHashCode());
                hash = hash * 23 + (PreferredVendorStatus == default(bool) ? 0 : PreferredVendorStatus.GetHashCode());
                hash = hash * 23 + (PurchasingWebServiceUrl == null ? 0 : PurchasingWebServiceUrl.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(Vendor left, Vendor right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vendor left, Vendor right)
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
