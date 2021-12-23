using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WCN_MVC.Generic
{
    public class GenericClass
    {
        List<SelectListItem> ddlYears = new List<SelectListItem>();
        public int minPage { get; set; } = 1;

        public int maxPage { get; set; } = 3;

        public DataTable ConvertToDataTable<T>(IList<T> data, string tableName)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            table.TableName = tableName;
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            table.AcceptChanges();
            return table;
        }


        public SelectList GetYears(int? iSelectedYear)
        {
            int CurrentYear = DateTime.Now.Year;
            int yearId = 0;

            for (int i = 2016; i <= CurrentYear; i++)
            {
                yearId = yearId + 1;
                ddlYears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                    //yearId.ToString()
                    //i.ToString()
                });
            }

            //Default It will Select Current Year  
            return new SelectList(ddlYears, "Value", "Text", iSelectedYear);

        }


    }
}