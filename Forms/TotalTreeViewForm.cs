using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace neoStockMasterv2.Forms
{
    public partial class TotalTreeViewForm : Form
    {
        // lwDisc içindeki sabit indeksler
        private const int COL_CHECKBOX = 0;
        private const int COL_NAME = 1;
        private const int COL_TOTAL = 2;
        private const int COL_CURRENCY = 3;
        private const int COL_DISC_STATUS = 4;
        private const int COL_DISC_AMOUNT = 5;
        private const int COL_SCT = 6;
        private const int COL_VAT = 7;

        public TotalTreeViewForm(ListView mainList, ListView detailList, bool isDiscount = false, bool isTax = false)
        {
            InitializeComponent();

            var tree = new TreeView { Dock = DockStyle.Fill };
            this.Controls.Add(tree);

            foreach (ListViewItem mainItem in mainList.Items)
            {
                // Güvenli şekilde ana başlığı al
                string mainCurrency = mainItem.SubItems.Count > 0 ? mainItem.SubItems[0].Text.Trim() : string.Empty;
                string mainTotal = mainItem.SubItems.Count > 1 ? mainItem.SubItems[1].Text.Trim() : string.Empty;

                var mainNode = new TreeNode($"{mainCurrency}: {mainTotal}");
                tree.Nodes.Add(mainNode);

                if (isDiscount)
                {
                    // İndirim detayları (lwDisc içinden)
                    foreach (ListViewItem detail in detailList.Items)
                    {
                        if (detail.SubItems.Count <= COL_CURRENCY) continue;
                        if (!string.Equals(detail.SubItems[COL_CURRENCY].Text.Trim(), mainCurrency, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string prod = detail.SubItems.Count > COL_NAME ? detail.SubItems[COL_NAME].Text.Trim() : string.Empty;
                        string disc = detail.SubItems.Count > COL_DISC_AMOUNT ? detail.SubItems[COL_DISC_AMOUNT].Text.Trim() : string.Empty;

                        if (!string.IsNullOrEmpty(prod))
                            mainNode.Nodes.Add($"{prod} - İndirim: {disc}");
                    }
                }
                else if (isTax)
                {
                    foreach (ListViewItem taxDetail in detailList.Items)
                    {
                        // Ürün adı
                        string productName = taxDetail.SubItems[COL_NAME].Text.Trim();

                        if (string.IsNullOrEmpty(productName))
                            continue;

                        string sctText = taxDetail.SubItems[COL_SCT].Text.Trim();
                        string vatText = taxDetail.SubItems[COL_VAT].Text.Trim();

                        // Her ürün için node
                        var productNode = new TreeNode(productName);

                        if (!string.IsNullOrWhiteSpace(sctText) &&
                            !sctText.Contains("Yok", StringComparison.OrdinalIgnoreCase) &&
                            !sctText.Contains("No", StringComparison.OrdinalIgnoreCase))
                        {
                            productNode.Nodes.Add($"ÖTV: {sctText}");
                        }

                        if (!string.IsNullOrWhiteSpace(vatText) &&
                            !vatText.Contains("Yok", StringComparison.OrdinalIgnoreCase) &&
                            !vatText.Contains("No", StringComparison.OrdinalIgnoreCase))
                        {
                            productNode.Nodes.Add($"KDV: {vatText}");
                        }

                        if (productNode.Nodes.Count > 0)
                            mainNode.Nodes.Add(productNode);
                    }
                }
                else
                {
                    // Toplam detay görünümü
                    foreach (ListViewItem detail in detailList.Items)
                    {
                        if (detail.SubItems.Count <= COL_CURRENCY) continue;
                        if (!string.Equals(detail.SubItems[COL_CURRENCY].Text.Trim(), mainCurrency, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string prod = detail.SubItems.Count > COL_NAME ? detail.SubItems[COL_NAME].Text.Trim() : string.Empty;
                        string total = detail.SubItems.Count > COL_TOTAL ? detail.SubItems[COL_TOTAL].Text.Trim() : string.Empty;

                        if (!string.IsNullOrEmpty(prod))
                            mainNode.Nodes.Add($"{prod}: {total}");
                    }
                }
            }

            tree.ExpandAll();
        }
    }
}  

