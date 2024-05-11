using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private const string apiUrl = "https://localhost:7239/api/VillaAPI";
        private HttpClient httpClient;

        public Form1()
        {
            InitializeComponent();
            httpClient = new HttpClient();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadVillas();
        }

        private async Task LoadVillas()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    List<Villa> villas = JsonConvert.DeserializeObject<List<Villa>>(json);
                    DisplayVillas(villas);
                }
                else
                {
                    MessageBox.Show("Failed to load villas: " + response.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load villas: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayVillas(List<Villa> villas)
        {
            listViewVillas.Items.Clear();

            foreach (var villa in villas)
            {
                var item = new ListViewItem(villa.Id.ToString());
                item.SubItems.Add(villa.Name);
                item.SubItems.Add(villa.Details);
                item.SubItems.Add(villa.Rate.ToString());
                item.SubItems.Add(villa.Sqft.ToString());
                item.SubItems.Add(villa.Occupancy.ToString());
                item.SubItems.Add(villa.ImageUrl);
                item.SubItems.Add(villa.Amenity);
                listViewVillas.Items.Add(item);
            }
        }

        private async void btnAdd_Click(object sender, EventArgs e)
        {
            var newVilla = new Villa
            {
                Name = txtName.Text,
                Details = txtDetails.Text,
                Rate = decimal.Parse(txtRate.Text),
                Sqft = int.Parse(txtSqft.Text),
                Occupancy = int.Parse(txtOccupancy.Text),
                ImageUrl = txtImageUrl.Text,
                Amenity = txtAmenity.Text
            };

            await CreateVilla(newVilla);
            await LoadVillas();
        }

        private async Task CreateVilla(Villa villa)
        {
            try
            {
                string json = JsonConvert.SerializeObject(villa);
                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, new StringContent(json, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to add villa: " + response.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add villa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (listViewVillas.SelectedItems.Count > 0)
            {
                int villaId = int.Parse(listViewVillas.SelectedItems[0].Text);
                var updatedVilla = new Villa
                {
                    Id = villaId,
                    Name = txtName.Text,
                    Details = txtDetails.Text,
                    Rate = decimal.Parse(txtRate.Text),
                    Sqft = int.Parse(txtSqft.Text),
                    Occupancy = int.Parse(txtOccupancy.Text),
                    ImageUrl = txtImageUrl.Text,
                    Amenity = txtAmenity.Text
                };

                await UpdateVilla(updatedVilla);
                await LoadVillas();
            }
            else
            {
                MessageBox.Show("Please select a villa to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewVillas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewVillas.SelectedItems.Count > 0)
            {
                var selectedItem = listViewVillas.SelectedItems[0];
                txtSearchId.Text = selectedItem.SubItems[0].Text; 
                txtName.Text = selectedItem.SubItems[1].Text;
                txtDetails.Text = selectedItem.SubItems[2].Text;
                txtRate.Text = selectedItem.SubItems[3].Text;
                txtSqft.Text = selectedItem.SubItems[4].Text;
                txtOccupancy.Text = selectedItem.SubItems[5].Text;
                txtImageUrl.Text = selectedItem.SubItems[6].Text;
            }
        }


        private async Task UpdateVilla(Villa villa)
        {
            try
            {
                string json = JsonConvert.SerializeObject(villa);
                HttpResponseMessage response = await httpClient.PutAsync($"{apiUrl}/{villa.Id}", new StringContent(json, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to update villa: " + response.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update villa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (listViewVillas.SelectedItems.Count > 0)
            {
                int villaId = int.Parse(listViewVillas.SelectedItems[0].Text);
                await DeleteVilla(villaId);
                await LoadVillas();
            }
            else
            {
                MessageBox.Show("Please select a villa to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task DeleteVilla(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.DeleteAsync($"{apiUrl}/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Failed to delete villa: " + response.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete villa: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnShow_Click(object sender, EventArgs e)
        {
            listViewVillas.Items.Clear();
            await LoadVillas();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {

            if (int.TryParse(txtSearchId.Text, out int searchId))
            {
                Villa villa = await GetVillaById(searchId);
                if (villa != null)
                {
                    listViewVillas.Items.Clear();

                    var item = new ListViewItem(villa.Id.ToString());
                    item.SubItems.Add(villa.Name);
                    item.SubItems.Add(villa.Details);
                    item.SubItems.Add(villa.Rate.ToString());
                    item.SubItems.Add(villa.Sqft.ToString());
                    item.SubItems.Add(villa.Occupancy.ToString());
                    item.SubItems.Add(villa.ImageUrl);
                    listViewVillas.Items.Add(item);

                    txtName.Text = villa.Name;
                    txtDetails.Text = villa.Details;
                    txtRate.Text = villa.Rate.ToString();
                    txtSqft.Text = villa.Sqft.ToString();
                    txtOccupancy.Text = villa.Occupancy.ToString();
                    txtImageUrl.Text = villa.ImageUrl;
                }
                else
                {
                    txtSearchId.Clear();
                    txtName.Clear();
                    txtDetails.Clear();
                    txtRate.Clear();
                    txtSqft.Clear();
                    txtOccupancy.Clear();
                    txtImageUrl.Clear();
                }
            }
            else
            {
                MessageBox.Show("Invalid id format. Please enter a valid integer id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Villa> GetVillaById(int id)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync($"{apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Villa villa = JsonConvert.DeserializeObject<Villa>(json);
                    return villa;
                }
                else
                {
                    MessageBox.Show($"Failed to get villa with id {id}: " + response.ReasonPhrase, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get villa with id {id}: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }


    }


    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public decimal Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
