using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Configuration.EntityFramework
{
    public class SettingEntity
    {
        private object _value;
        private string _json;

        [Required]
        [Key]
        public virtual int Id { get; set; }

        [Required]
        public virtual int SectionId { get; set; }

        [Required]
        [MaxLength(50)]
        public virtual string Key { get; set; }

        public virtual SectionEntity Section { get; set; }

        public virtual string ValueType { get; set; }

        public virtual string DefaultValue { get; set; }

        public virtual T GetValue<T>()
        {
            return (T)Convert.ChangeType(this.GetValue(typeof(T)), typeof(T));
        }

        public virtual object GetValue(Type type)
        {
            if ((_value == null) && !string.IsNullOrEmpty(this.Json))
            {
                _value = JsonConvert.DeserializeObject(this.Json, type);
            }
            return _value;
        }

        public virtual void SetValue(object value)
        {
            if (_value != value)
            {
                _value = value;
                this.ValueType = null;
                this.Json = null;
                if (value != null)
                {
                    this.Json = JsonConvert.SerializeObject(value);
                    if (string.IsNullOrEmpty(this.DefaultValue))
                    {
                        this.DefaultValue = this.Json;
                    }
                    var name = value.GetType().AssemblyQualifiedName.Split(Convert.ToChar(","));
                    ValueType = $"{name[0].Trim()}, {name[1].Trim()}";
                }
            }
        }

        public virtual string Json
        {
            get { return this._json; }
            set
            {
                this._json = value;
                this._value = null;
            }
        }

        public virtual DateTime? Modified { get; set; }

        [MaxLength(50)]
        public virtual string ModifiedUser { get; set; }

        [Required]
        public virtual byte[] Timestamp { get; set; }
    }
}