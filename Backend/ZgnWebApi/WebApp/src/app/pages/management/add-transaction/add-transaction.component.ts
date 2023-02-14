import { MessagerService } from 'src/app/services/messager.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var initializeOptionData: any;
declare var w2popup: any;
@Component({
  selector: 'app-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.css'],
})
export class AddTransactionComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(private messagerService:MessagerService) {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.AddTransactionLayout) {
      w2ui.AddTransactionLayout.destroy();
    }
    $().w2layout({
      name: 'AddTransactionLayout',
      panels: [
        {
          layoutName: 'AddTransactionLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#AddTransactionLayout').w2render('AddTransactionLayout');
    if(w2ui.AddTransactionForm) {
      w2ui.AddTransactionForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'AddTransactionForm',
      header:'Talep Oluştur',
      autoSize: true,
      tabs: [
        { id: 'addTransaction', text: 'Talep Oluştur' },
        { id: 'returnTransaction', text: 'İade Talebi Oluştur' }
      ],
      fields: [
        {
          type: 'list',
          field: 'GroupCodeView',
          required:true,
          html: {
            label: 'Grup',
          },
          options: { items: [], openOnFocus: true, match: 'contain',markSearch: true, filter: true },
        },
        {
          type: 'list',
          field: 'ProductIdView',
          required:true,
          html: {
            label: 'Ürün',
          },
          options: { items: [], openOnFocus: true, match: 'contain',markSearch: true, filter: true  },
        },
        {
          type: 'text',
          field: 'ProductCode',
          required:true,
          disabled:true,
          html: {
            label: 'Ürün Kodu',
          },
        },
        {
          type: 'list',
          field: 'SerialNumberView',
          required: false,
          hidden:true,
          html: {
            label: 'Seri Numarası',
          },
          options: { items: [], openOnFocus: true, match: 'contain',markSearch: true, filter: true  },
        },
        {
          type: 'text',
          field: 'LocationName',
          required:false,
          disabled: true,
          hidden: true,
          html: {
            label: 'Lokasyon Adı',
          },
        },
        {
          type: 'list',
          field: 'ToNodeView',
          required:true,
          html: {
            label: 'Teslimat Noktası',
          },
          options: { items: [], openOnFocus: true, match: 'contain',markSearch: true, filter: true  },
        },
        {
          type: 'textarea',
          field: 'Description',
          html: {
            label: 'Açıklama',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'Transactions/GetForUi',
        save: this.apiUrl + 'Transactions/AddForUi',
      },
      onChange: function (event: any) {
        if (event.target == 'GroupCodeView') {
          var groupCode = event.value_new?.id;
          w2ui.AddTransactionForm.record.ProductCode=null;
          w2ui.AddTransactionForm.record.LocationName=null;
          w2ui.AddTransactionForm.lock('')
          initializeOptionData('AddTransactionForm','ProductIdView',self.apiUrl+'Klimasan/GetProductsByGroupCode/'+groupCode,'ProductId','Ürün Kodu: {ProductCode} - Ürün Adı: {ProductName}');
        }
        if (event.target == 'ProductIdView') {
          var groupCode = event.value_new?.id;
          w2ui.AddTransactionForm.record.ProductCode = event.value_new?.obj.ProductCode;
          w2ui.AddTransactionForm.record.LocationName=null;
          w2ui.AddTransactionForm.lock('')
          initializeOptionData('AddTransactionForm', 'SerialNumberView', self.apiUrl + 'Klimasan/GetInventoriesByProductId/' + groupCode, 'SerialNumber', 'Seri Numarası: {SerialNumber} - Lokasyon Adı: {LocationName} - Adet:{UnitQuantity}');
          initializeOptionData('AddTransactionForm', 'ToNodeView', self.apiUrl + 'BlueBotics/GetAllDropNodesByLoginUser', 'Name', 'Name');
        }
        if (event.target == 'SerialNumberView') {
          var groupCode = event.value_new?.id;
          w2ui.AddTransactionForm.record.LocationName = event.value_new?.obj.LocationName;
          w2ui.AddTransactionForm.lock('')
          initializeOptionData('AddTransactionForm','ToNodeView',self.apiUrl+'BlueBotics/GetAllDropNodesByLoginUser','Name','Name');
        }
      },
      onSave: function (event: any) {
        self.messagerService.success('Talep Oluşturuldu');
        // w2ui.AddTransactionForm.clear();
      },
      actions: {
        btnSave: {
          text: 'Ekle',
          onClick(event: any) {
            w2ui.AddTransactionForm.save();
          },
        },
      },
    });
    w2ui.AddTransactionForm.lock('')
    initializeOptionData('AddTransactionForm','GroupCodeView',self.apiUrl+'Klimasan/GetAllGroupsByLoginUser','GroupCode','GroupCode');
    w2ui.AddTransactionLayout.html('main', w2ui.AddTransactionForm);
  }
}
