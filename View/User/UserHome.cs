using ASM1MVC.Controller;
using ASM1MVC.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM1MVC.View.User
{
    public partial class UserHome : Form
    {
        public BookController tempListBook = new BookController();
        public RentBookController listUserRentBook = new RentBookController();

        public UserHome()
        {
            InitializeComponent();
        }
        public UserHome(String value, int CodeUser)
        {
            InitializeComponent();
            InitData();
            LoadData();
            LoadCategory();
            lblName.Text = value;
            lblIDUser.Text = CodeUser.ToString();
        }
        public UserHome(String value, int CodeUser, int money)
        {
            InitializeComponent();
            InitData();
            LoadData();
            LoadCategory();
            lblName.Text = value;
            lblIDUser.Text = CodeUser.ToString();
            txtMoney.Text = money.ToString();
        }
        public void InitData()
        {
            tempListBook.clearList();
            tempListBook.LoadData();
        }
        public void LoadData()
        {
            InitData();
            datagvBooks.DataSource = null;
            datagvBooks.DataSource = tempListBook.GetListBook();
            //int sizeWidth = 120;
            //for (int i = 0; i < 6; i++)
            //{
            //    datagvBooks.Columns[i].Width = sizeWidth;
            //}
        }
        public void LoadCategory()
        {
            CategoryController tempListCategory = new CategoryController();
            tempListCategory.LoadData();
            foreach (var tempCategory in tempListCategory.listCategory)
            {
                cbCategory.Items.Add(tempCategory.CategoryName);
            }
        }
        public void Rentbook()
        {
            int tempMoney = Convert.ToInt32(txtMoney.Text);
            int price = Convert.ToInt32(txtPrice.Text);
            int Total = tempMoney - price;
            UserModel tempUser = new UserModel();
            tempUser.UserID = Convert.ToInt32(lblIDUser.Text);
            tempUser.Money = Total;
            listUserRentBook.UserRentBook(tempUser);
            txtMoney.Text = Total.ToString();

            // reduce book
            int numBook = Convert.ToInt32(txtNumLeft.Text);
            Book tempBook = new Book();
            tempBook.BookID = Convert.ToInt32(txtCode.Text);
            tempBook.NumberOfBookLeft = numBook - 1;
            tempListBook.ReduceBook(tempBook);
            LoadData();
        }
        public Boolean CheckMoney()
        {
            int tempMoney = Convert.ToInt32(txtMoney.Text);
            int price = Convert.ToInt32(txtPrice.Text);
            int Total = tempMoney - price;
            if (Total < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean CheckNumBook()
        {
            int tempNumBook = Convert.ToInt32(txtNumLeft.Text);
            if (tempNumBook > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RentBook tempBook = new RentBook();
                tempBook.BookIDRentBook = Convert.ToInt32(txtCode.Text);
                tempBook.UserIDRentBook = Convert.ToInt32(lblIDUser.Text);
                //Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                tempBook.TimeRentBook = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string message = $" do you want to rent this book ?\nID Book = '{tempBook.BookIDRentBook}' \nID User = '{tempBook.UserIDRentBook}' \nTime rent book = '{tempBook.TimeRentBook}'";
                string caption = " Confirmation Delete";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                if (txtCode.Text != "")
                {
                    DialogResult result = MessageBox.Show(message, caption, buttons, icon);
                    if (result == DialogResult.Yes)
                    {
                        if(CheckNumBook() == false)
                        {
                            string messageNoMoney = $"This book is over, please come back later.";
                            MessageBox.Show(messageNoMoney);
                        }
                        else if (CheckMoney() == false)
                        {
                            Rentbook();
                            listUserRentBook.Add(tempBook);
                            LoadData();
                            string messageNoMoney = $"You have successfully rented this book. \n your remaining amount is '{txtMoney.Text}'";
                            MessageBox.Show(messageNoMoney);
                        }
                        else
                        {
                            string messageNoMoney = $"You don't have enough money to rent this book. \n\n" +
                                $"the amount you have is '{txtMoney.Text}'and the price of the book is '{txtPrice.Text}'.\n\n" +
                                $"You need {Convert.ToInt32(txtPrice.Text) - Convert.ToInt32(txtMoney.Text)} extra money to buy this book.";
                            MessageBox.Show(messageNoMoney);
                        }
                    }
                }
            }
            catch
            {
            }
           
        }

        private void runOptions_Click(object sender, EventArgs e)
        {
            if(cbOptions.SelectedItem.ToString() == "Search By Book ID")
            {
                tempListBook.clearList();
                Book tempBook = new Book();
                tempBook.BookID = Convert.ToInt16(txtOptions.Text);
                tempListBook.SearchBookByID(tempBook);
                datagvBooks.DataSource = null;
                datagvBooks.DataSource = tempListBook.GetListBook();

            } else if (cbOptions.SelectedItem.ToString() == "View All Book")
            {
                LoadData();
            }
            else 
            {
                tempListBook.clearList();
                Book tempBook = new Book();
                tempBook.Author = txtOptions.Text;
                tempListBook.SearchBookByAuthor(tempBook);
                datagvBooks.DataSource = null;
                datagvBooks.DataSource = tempListBook.GetListBook();
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Role tempRole = new Role();
            tempRole.Show();
        }

        private void datagvBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Text = datagvBooks.CurrentRow.Cells[0].Value.ToString();         // CurrentRow la cai click hien tai. Cell la o^ thu may. lay' value va chuyen sang string  
            txtTitle.Text = datagvBooks.CurrentRow.Cells[1].Value.ToString();
            txtPublisher.Text = datagvBooks.CurrentRow.Cells[2].Value.ToString();
            txtEdition.Text = datagvBooks.CurrentRow.Cells[3].Value.ToString();
            txtAuthor.Text = datagvBooks.CurrentRow.Cells[4].Value.ToString();
            cbCategory.SelectedItem = datagvBooks.CurrentRow.Cells[9].Value.ToString();
            txtNumLeft.Text = datagvBooks.CurrentRow.Cells[6].Value.ToString();
            txtPrice.Text = datagvBooks.CurrentRow.Cells[7].Value.ToString();
            string exePath = Application.StartupPath;
            img1.ImageLocation = exePath + "\\images\\" +
                datagvBooks.CurrentRow.Cells[5].Value.ToString();
            img1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void cbOptions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            UserRentBook tempUserRentBook = new UserRentBook(lblName.Text,Convert.ToInt16(lblIDUser.Text),Convert.ToInt32(txtMoney.Text));
            tempUserRentBook.Show();
        }

        private void txtOptions_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
