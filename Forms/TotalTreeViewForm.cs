using neoStockMasterv2.Data.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace neoStockMasterv2.Forms
{
    public partial class TotalTreeViewForm : Form
    {
        // lwDisc içindeki sabit indeksler
        private const int COL_NAME = 1;
        private const int COL_TOTAL = 2;
        private const int COL_CURRENCY = 3;
        private const int COL_DISC_AMOUNT = 5;
        private const int COL_SCT = 6;
        private const int COL_VAT = 7;

        public TotalTreeViewForm(ListView mainList, ListView detailList, bool isDiscount = false, bool isTax = false)
        {
            InitializeComponent();

            string discLabel = LanguageService.T("İndirim", "Discount");
            string sctLabel = LanguageService.T("ÖTV", "SCT");
            string vatLabel = LanguageService.T("KDV", "VAT");

            var tree = new TreeView { Dock = DockStyle.Fill };
            this.Controls.Add(tree);

            // OrderViewEditScreen'de ÖTV / KDV ayrı satırlarda; PricingOrderScreen'de tek TL satırı var
            bool hasSplitTaxRows = mainList.Items.Cast<ListViewItem>()
                .Any(i => IsSctRow(i.Text) || IsVatRow(i.Text));

            foreach (ListViewItem mainItem in mainList.Items)
            {
                string mainText = Cell(mainItem, 0);
                string mainTotal = Cell(mainItem, 1);

                var mainNode = new TreeNode($"{mainText}: {mainTotal}");
                tree.Nodes.Add(mainNode);

                if (isDiscount)
                {
                    foreach (ListViewItem detail in detailList.Items)
                    {
                        if (!SameText(Cell(detail, COL_CURRENCY), mainText)) continue;

                        string prod = Cell(detail, COL_NAME);
                        if (prod != "")
                            mainNode.Nodes.Add($"{prod} - {discLabel}: {Cell(detail, COL_DISC_AMOUNT)}");
                    }
                }
                else if (isTax)
                {
                    bool showSct = hasSplitTaxRows ? IsSctRow(mainText) : true;
                    bool showVat = hasSplitTaxRows ? IsVatRow(mainText) : true;
                    if (!showSct && !showVat) continue;   // TOPLAM satırı: alt dal yok

                    foreach (ListViewItem detail in detailList.Items)
                    {
                        string prod = Cell(detail, COL_NAME);
                        if (prod == "") continue;

                        string sctText = Cell(detail, COL_SCT);
                        string vatText = Cell(detail, COL_VAT);

                        if (showSct && showVat)
                        {
                            // Tek satırlı görünüm: ürün altında ÖTV ve KDV
                            var productNode = new TreeNode(prod);
                            if (HasTax(sctText)) productNode.Nodes.Add($"{sctLabel}: {sctText}");
                            if (HasTax(vatText)) productNode.Nodes.Add($"{vatLabel}: {vatText}");
                            if (productNode.Nodes.Count > 0) mainNode.Nodes.Add(productNode);
                        }
                        else if (showSct && HasTax(sctText))
                        {
                            mainNode.Nodes.Add($"{prod}: {sctText}");
                        }
                        else if (showVat && HasTax(vatText))
                        {
                            mainNode.Nodes.Add($"{prod}: {vatText}");
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem detail in detailList.Items)
                    {
                        if (!SameText(Cell(detail, COL_CURRENCY), mainText)) continue;

                        string prod = Cell(detail, COL_NAME);
                        if (prod != "")
                            mainNode.Nodes.Add($"{prod}: {Cell(detail, COL_TOTAL)}");
                    }
                }
            }

            tree.ExpandAll();
        }

        // Güvenli hücre okuma (sütun yoksa "")
        private static string Cell(ListViewItem item, int index) =>
            item.SubItems.Count > index ? item.SubItems[index].Text.Trim() : string.Empty;

        private static bool SameText(string a, string b) =>
            string.Equals(a, b, StringComparison.OrdinalIgnoreCase);

        private static bool IsSctRow(string text) =>
            text.StartsWith("ÖTV", StringComparison.OrdinalIgnoreCase) || text.StartsWith("SCT", StringComparison.OrdinalIgnoreCase);

        private static bool IsVatRow(string text) =>
            text.StartsWith("KDV", StringComparison.OrdinalIgnoreCase) || text.StartsWith("VAT", StringComparison.OrdinalIgnoreCase);

        // "ÖTV Yok", "KDV Yok", "No VAT", "No Excise Tax" gibi değerler vergi sayılmaz
        private static bool HasTax(string text) =>
            !string.IsNullOrWhiteSpace(text) &&
            !text.Contains("Yok", StringComparison.OrdinalIgnoreCase) &&
            !text.StartsWith("No ", StringComparison.OrdinalIgnoreCase);
    }
}