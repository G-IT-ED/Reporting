﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using Reporting.Model;

namespace Reporting
{
    public partial class Form1 : Form
    {
        private BindingList<ReportView> _bindingList = new BindingList<ReportView>();
        private string _config;

        public Form1()
        {
            InitializeComponent();
            _dateStart.EditValue = DateTime.Now;
            _dateEnd.EditValue = DateTime.Now;
            ReadConfig();
        }

        private void ReadConfig()
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(@"config.txt");
            while ((line = file.ReadLine()) != null)
            {
                _config = line;
            }
            file.Close();
        }

        private void GetData(DateTime start, DateTime finish)
        {
            DataMapper dataMapper = new DataMapper(_config);
            _bindingList.Add(new ReportView("Количество брикетов, приходящих на спуске ленточного конвейера", dataMapper.CountComingDownConveyor(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отбракованных по весу", dataMapper.CountCulledByWeight(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отбракованных на металлодетекторе", dataMapper.CountCulledMetalDetector(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих через пленкообертку", dataMapper.CountPassingFilmWrapper(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих систему взвешивания", dataMapper.CountPassingWeighingSystem(start,finish)));
            _bindingList.Add(new ReportView("Количество брикетов, проходящих зону штатного появления 1 и  зону штатного появления 2", dataMapper.CountPassingZoneRegularAppearance(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, отправленных на склад", dataMapper.CountSentToWarehouse(start,finish)));
            _bindingList.Add(new ReportView("Kоличество брикетов, проходящих проверку на наличие металла", dataMapper.CountTestedPresenceOfMetal(start,finish)));
        }

        private async void _viewButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SplashScreenManager.ShowForm(typeof(SplashScreen1));
            _bindingList.Clear();
            await ShowData();
            SplashScreenManager.CloseForm();
        }

        private async Task ShowData()
        {
            try
            {                
                DateTime start = (DateTime)_dateStart.EditValue;
                DateTime finish = (DateTime)_dateEnd.EditValue;
                if (start > finish)
                {
                    MessageBox.Show("Дата начала должна быть раньше даты окончания!");
                    return;
                }
                GetData(start, finish);
                gridControl1.DataSource = _bindingList;                
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message, "Не удалось загрузить данные!");
            }
        }

        private async void ExportButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await Export();
        }

        private async Task Export()
        {
            if (_bindingList.Count == 0)
            {
                MessageBox.Show("Отсутсвуют данные для выгрузки! Выберите требуемую дату, затем выберите пункт просмотр.");
                return;
            }

            //выбираем путь
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "",
                Title = @"Save as Excel File",
                Filter = @"Excel (2007)(.xlsx)|*.xlsx|Excel (2010)(.xlsx)|*.xlsx",
                OverwritePrompt = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            gridControl1.ExportToXlsx(saveFileDialog.FileName);
        }

        private void _exitButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private async void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await Export();
        }

        private async void _exportBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            await Export();
        }
    }
}
