using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Reporting.Model;

namespace Reporting
{
    public partial class Form1 : Form
    {
        private BindingList<ReportView> _bindingList = new BindingList<ReportView>();

        public Form1()
        {
            InitializeComponent();
        }

        private void GetData(DateTime start, DateTime finish)
        {
            DataMapper dataMapper = new DataMapper();
            _bindingList.Add(new ReportView("Количество брикетов, приходящих на спуске ленточного конвейера", dataMapper.CountComingDownConveyor(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отбракованных по весу", dataMapper.CountCulledByWeight(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отбракованных на металлодетекторе", dataMapper.CountCulledMetalDetector(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих через пленкообертку", dataMapper.CountPassingFilmWrapper(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих систему взвешивания", dataMapper.CountPassingWeighingSystem(start,finish)));
            _bindingList.Add(new ReportView("Количество брикетов, проходящих зону штатного появления 1 и  зону штатного появления 2", dataMapper.CountPassingZoneRegularAppearance(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отправленных на склад", dataMapper.CountSentToWarehouse(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих проверку на наличие металла", dataMapper.CountTestedPresenceOfMetal(start,finish)));
        }

        private void _viewButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DateTime start = (DateTime)_dateStart.EditValue;
            DateTime finish = (DateTime)_dateEnd.EditValue;
            GetData(start, finish);
            gridControl1.DataSource = _bindingList;
        }

        private void ExportButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //выбираем путь
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "",
                Title = @"Save as Excel File",
                Filter = @"Excel Files(2003)(.xls)|*.xls|Excel (2007)(.xlsx)|*.xlsx|Excel (2010)(.xlsx)|*.xlsx",
                OverwritePrompt = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            gridControl1.ExportToXlsx(saveFileDialog.FileName);
        }
    }
}
