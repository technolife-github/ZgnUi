import { MessagerService } from 'src/app/services/messager.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var w2popup: any;
declare var openPopup: any;
declare var initializeOptionData: any;
@Component({
  selector: 'app-pending-transaction',
  templateUrl: './pending-transaction.component.html',
  styleUrls: ['./pending-transaction.component.css'],
})
export class PendingTransactionComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(private messagerService:MessagerService) {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.PendingTransactionLayout) {
      w2ui.PendingTransactionLayout.destroy();
    }
    $().w2layout({
      name: 'PendingTransactionLayout',
      panels: [
        {
          layoutName: 'PendingTransactionLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#PendingTransactionLayout').w2render('PendingTransactionLayout');
    if (w2ui.PendingTransactionGrid) {
      w2ui.PendingTransactionGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'PendingTransactionGrid',
      layout: 'PendingTransactionLayout',
      panel: 'main',
      show: {
        header: true,
        footer: true,
        toolbar: true,
        selectColumn: true,
      },
      toolbar: {
          items: [
            { id: 'start', type: 'button', text: 'İşlemi Tamamla', icon: 'fa fa-play' },
          ],
          onClick(event:any) {
              if (event.target == 'start') {
                var selection = w2ui.PendingTransactionGrid.getSelection();
                if (selection.length == 0) {
                  self.messagerService.error('Lütfen bir işlem seçiniz.');
                  return;
                }
                openPopup('İşlem Başlatma', 'StartTransactionForm',selection[0], {});
                initializeOptionData('StartTransactionForm','FromNodeView',self.apiUrl+'BlueBotics/GetAllPickNodesByLoginUser','Name','Name');
              }
          }
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Transactions/GetAllPendingForUi'
      },
      autoLoad: true,
      recid: 'Id',
      advanceOnEdit: true,
      columns: [
        {
          searchable: 'int',
          text: 'Id',
          field: 'Id',
          size: '100px',
        },
        {
          searchable: 'enum',
          text: 'Durum',
          field: 'Status',
          size: '100px',
          options: { items: ['Pending','Ready',"End"], openOnFocus: true },
        },
        {
          searchable: true,
          text: 'Alış Adresi',
          field: 'FromNode',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Teslimat Adresi',
          field: 'ToNode',
          size: '100px',
        },
        {
          searchable: 'int',
          text: 'İşlem Numarası',
          field: 'ProcessId',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Grup Kodu',
          field: 'GroupCode',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Ürün Numarası',
          field: 'ProductId',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Ürün Kodu',
          field: 'ProductCode',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Seri Numarası',
          field: 'SerialNumber',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Lokasyon Adı',
          field: 'LocationName',
          size: '100px',
        },
        {
          searchable: 'datetime',
          text: 'Başlangıç Tarihi',
          field: 'StartDate',
          size: '100px',
          options: { format: 'yyyy.mm.dd | h24' },
        },
        {
          searchable: 'datetime',
          text: 'Bitiş Tarihi',
          field: 'EndDate',
          size: '100px',
          options: { format: 'yyyy.mm.dd | h24' },
        },
      ],
    });
    w2ui.PendingTransactionLayout.html('main', w2ui.PendingTransactionGrid);
    if (w2ui.StartTransactionForm) {
      w2ui.StartTransactionForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'StartTransactionForm',
      autoSize: true,
      fields: [
        {
          type: 'int',
          text:'Id',
          field: 'Id',
          required:true,
          disabled: true,
        },
        {
          type: 'list',
          field: 'FromNodeView',
          required:true,
          html: {
            label: 'Alış Noktası',
          },
          options: { items: [], openOnFocus: true },
        },
      ],
      url: {
        get: this.apiUrl + 'Transactions/GetForUi',
        save: this.apiUrl + 'Transactions/StartForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.PendingTransactionGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Başlat',
          onClick(event: any) {
            w2ui.StartTransactionForm.save();
          },
        },
      },
    });
  }
}
