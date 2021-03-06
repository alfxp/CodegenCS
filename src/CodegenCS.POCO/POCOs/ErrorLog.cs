﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;
using System.ComponentModel;

namespace CodegenCS.AdventureWorksPOCOSample
{
    public partial class ErrorLog : INotifyPropertyChanged
    {
        #region Members
        private int _errorLogId;
        [Key]
        public int ErrorLogId 
        { 
            get { return _errorLogId; } 
            set { SetField(ref _errorLogId, value, nameof(ErrorLogId)); } 
        }
        private int? _errorLine;
        public int? ErrorLine 
        { 
            get { return _errorLine; } 
            set { SetField(ref _errorLine, value, nameof(ErrorLine)); } 
        }
        private string _errorMessage;
        public string ErrorMessage 
        { 
            get { return _errorMessage; } 
            set { SetField(ref _errorMessage, value, nameof(ErrorMessage)); } 
        }
        private int _errorNumber;
        public int ErrorNumber 
        { 
            get { return _errorNumber; } 
            set { SetField(ref _errorNumber, value, nameof(ErrorNumber)); } 
        }
        private string _errorProcedure;
        public string ErrorProcedure 
        { 
            get { return _errorProcedure; } 
            set { SetField(ref _errorProcedure, value, nameof(ErrorProcedure)); } 
        }
        private int? _errorSeverity;
        public int? ErrorSeverity 
        { 
            get { return _errorSeverity; } 
            set { SetField(ref _errorSeverity, value, nameof(ErrorSeverity)); } 
        }
        private int? _errorState;
        public int? ErrorState 
        { 
            get { return _errorState; } 
            set { SetField(ref _errorState, value, nameof(ErrorState)); } 
        }
        private DateTime _errorTime;
        public DateTime ErrorTime 
        { 
            get { return _errorTime; } 
            set { SetField(ref _errorTime, value, nameof(ErrorTime)); } 
        }
        private string _userName;
        public string UserName 
        { 
            get { return _userName; } 
            set { SetField(ref _userName, value, nameof(UserName)); } 
        }
        #endregion Members

        #region ActiveRecord
        public void Save()
        {
            if (ErrorLogId == default(int))
                Insert();
            else
                Update();
        }
        public void Insert()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                INSERT INTO [ErrorLog]
                (
                    [ErrorLine],
                    [ErrorMessage],
                    [ErrorNumber],
                    [ErrorProcedure],
                    [ErrorSeverity],
                    [ErrorState],
                    [ErrorTime],
                    [UserName]
                )
                VALUES
                (
                    @ErrorLine,
                    @ErrorMessage,
                    @ErrorNumber,
                    @ErrorProcedure,
                    @ErrorSeverity,
                    @ErrorState,
                    @ErrorTime,
                    @UserName
                )";

                this.ErrorLogId = conn.Query<int>(cmd + "SELECT SCOPE_IDENTITY();", this).Single();
            }
        }
        public void Update()
        {
            using (var conn = IDbConnectionFactory.CreateConnection())
            {
                string cmd = @"
                UPDATE [ErrorLog] SET
                    [ErrorLine] = @ErrorLine,
                    [ErrorMessage] = @ErrorMessage,
                    [ErrorNumber] = @ErrorNumber,
                    [ErrorProcedure] = @ErrorProcedure,
                    [ErrorSeverity] = @ErrorSeverity,
                    [ErrorState] = @ErrorState,
                    [ErrorTime] = @ErrorTime,
                    [UserName] = @UserName
                WHERE
                    [ErrorLogID] = @ErrorLogId";
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
            ErrorLog other = obj as ErrorLog;
            if (other == null) return false;

            if (ErrorLine != other.ErrorLine)
                return false;
            if (ErrorMessage != other.ErrorMessage)
                return false;
            if (ErrorNumber != other.ErrorNumber)
                return false;
            if (ErrorProcedure != other.ErrorProcedure)
                return false;
            if (ErrorSeverity != other.ErrorSeverity)
                return false;
            if (ErrorState != other.ErrorState)
                return false;
            if (ErrorTime != other.ErrorTime)
                return false;
            if (UserName != other.UserName)
                return false;
            return true;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (ErrorLine == null ? 0 : ErrorLine.GetHashCode());
                hash = hash * 23 + (ErrorMessage == null ? 0 : ErrorMessage.GetHashCode());
                hash = hash * 23 + (ErrorNumber == default(int) ? 0 : ErrorNumber.GetHashCode());
                hash = hash * 23 + (ErrorProcedure == null ? 0 : ErrorProcedure.GetHashCode());
                hash = hash * 23 + (ErrorSeverity == null ? 0 : ErrorSeverity.GetHashCode());
                hash = hash * 23 + (ErrorState == null ? 0 : ErrorState.GetHashCode());
                hash = hash * 23 + (ErrorTime == default(DateTime) ? 0 : ErrorTime.GetHashCode());
                hash = hash * 23 + (UserName == null ? 0 : UserName.GetHashCode());
                return hash;
            }
        }
        public static bool operator ==(ErrorLog left, ErrorLog right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ErrorLog left, ErrorLog right)
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
