import { BlueBoticsService } from './../../../services/blue-botics.service';
import { MessagerService } from 'src/app/services/messager.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var w2utils: any;
declare var w2popup: any;
declare var openPopupGrid: any;
declare var openSelectedPopupGrid: any;
declare var initializeOptionData: any;
@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css'],
})
export class TransactionComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(private messagerService:MessagerService,private blueBoticsService:BlueBoticsService) {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.TransactionLayout) {
      w2ui.TransactionLayout.destroy();
    }
    $().w2layout({
      name: 'TransactionLayout',
      panels: [
        {
          layoutName: 'TransactionLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#TransactionLayout').w2render('TransactionLayout');
    if (w2ui.TransactionGrid) {
      w2ui.TransactionGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'TransactionGrid',
      layout: 'TransactionLayout',
      panel: 'main',
      show: {
        header: true,
        footer: true,
        toolbar: true
      },
      toolbar: {
          items: [
            { id: 'viewLogs', type: 'button', text: 'Logları Göster', icon: 'fa fa-eye' },
            { id: 'bluebotics', type: 'menu', text: 'BlueBotics', icon: 'fa fa-linode', items: [
              { text: 'İşlemi Getir', id: 'get-mission' },
              { text: 'İşlemleri Getir', id: 'get-missions' },
              { text: 'İşlemi İptal Et', id: 'delete-mission' },
              { text: 'İşlemleri İptal Et', id: 'delete-missions' },
          ] },
          ],
          onClick(event:any) {
            if (event.target == 'viewLogs') {
              var selection = w2ui.TransactionGrid.getSelection();
              if (selection.length == 0) {
                self.messagerService.error('Lütfen bir işlem seçiniz.');
                return;
              }
              openPopupGrid('Log Kayıtları', 'LogGrid',{TransactionID:selection[0]});
            }
            else if (event.target == 'bluebotics:get-mission') {
              var selection = w2ui.TransactionGrid.getSelection();
              if (selection.length == 0) {
                self.messagerService.simple('Lütfen bir işlem seçiniz.','warning');
                return;
              }
              let rec = w2ui.TransactionGrid.get(selection[0]);
              w2ui.GetMissionGrid.header = rec.Id+' numaralı kayıda ait işlem detayları';
              openPopupGrid('İşlem Detayları', 'GetMissionGrid',{TransactionID:selection[0]});
            }
            else if (event.target == 'bluebotics:get-missions') {
              openPopupGrid('İşlemler', 'GetMissionsGrid');
            }
            else if (event.target == 'bluebotics:delete-mission') {
              var selection = w2ui.TransactionGrid.getSelection();
              if (selection.length == 0) {
                self.messagerService.simple('Lütfen bir işlem seçiniz.','warning');
                return;
              }
              let rec = w2ui.TransactionGrid.get(selection[0]);
              self.cancelMission(rec.ProcessId);
            }
            else if (event.target == 'bluebotics:delete-missions') {
              self.cancelMissions();
            }
          }
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Transactions/GetAllForUi',
        remove: this.apiUrl + 'Transactions/SoftDeleteForUi',
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
    w2ui.TransactionLayout.html('main', w2ui.TransactionGrid);

    if (w2ui.LogGrid) {
      w2ui.LogGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'LogGrid',
      show: {
        header: true,
        footer: true,
        toolbar: true
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Logs/GetAllByTransactionIdUi'
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
          searchable: true,
          text: 'Log Tipi',
          field: 'Type',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Log',
          field: 'LogText',
          size: '100px',
        },
        {
          searchable: 'datetime',
          text: 'Oluşturulduğu Tarih',
          field: 'CreatedAt',
          size: '100px',
          options: { format: 'yyyy.mm.dd | h24' },
        },
      ],
    });
    if (w2ui.GetMissionGrid) {
        w2ui.GetMissionGrid.destroy();
      }
      $().w2grid({
        name: 'GetMissionGrid',
        method: 'GET',
        url: {
          get: this.apiUrl + 'Transactions/GetMissionByTransactionIdUi'
        },
        show: { header: true, columnHeaders: false },
        columns: [
            { field: 'Name', size: '160px', style: 'background-color: #efefef; border-bottom: 1px solid white; padding-right: 5px;', attr: "align=right" },
            { field: 'Value', size: '100%' }
        ]
    });

    if (w2ui.GetMissionsGrid) {
      w2ui.GetMissionsGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'GetMissionsGrid',
      show: {
        header: true,
        footer: true,
        toolbar: true
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'BlueBotics/GetAllMissionsUi'
      },
      autoLoad: true,
      recid: 'Id',
      advanceOnEdit: true,
      columns: [
        {
          searchable: true,
          text: 'Id',
          field: 'Missionid',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Durum',
          field: 'State',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Navigasyon Durumu',
          field: 'Navigationstate',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Taşıma Durumu',
          field: 'Transportstate',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Alış Adresi',
          field: 'Fromnode',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Teslimat Adresi',
          field: 'Tonode',
          size: '100px',
        },
        {
          searchable: 'toggle',
          text: 'Yük',
          field: 'Payload',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Yük Varmı?',
          field: 'Isloaded',
          size: '100px',
        },
        {
          searchable: 'int',
          text: 'Öncelik',
          field: 'Priority',
          size: '100px',
        },
        {
          searchable: 'int',
          text: 'Cihaz',
          field: 'Assignedto',
          size: '100px',
        },
        {
          searchable: 'datetime',
          text: 'Tahmini Bitiş Tarihi',
          field: 'Deadline',
          size: '100px',
          options: { format: 'yyyy.mm.dd | h24' },
        },
        {
          searchable: true,
          text: 'Görev Tipi',
          field: 'Missiontype',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Grup No',
          field: 'Groupid',
          size: '100px',
        },
        {
          searchable: 'toggle',
          text: 'Bugün mü?',
          field: 'Istoday',
          size: '100px',
        },
        {
          searchable: 'int',
          text: 'Görev Durumu',
          field: 'Schedulerstate',
          size: '100px',
        },
      ],
    });
  }
  cancelMission(missionId:any){
    this.blueBoticsService.cancelMission(missionId).subscribe(res => {
      if(res.RetCode == 0){
        w2ui.TransactionGrid.reload();
        this.messagerService.simple('Görev iptal edildi');
      }else{
        this.messagerService.error('Hata',res.Error);
      }
    },
    (error:any)=>{
      this.messagerService.error('Hata',error.error.Error);
    });
  }
  cancelMissions(){
    this.blueBoticsService.cancelMissions().subscribe(res => {
      if(res.RetCode == 0){
        w2ui.TransactionGrid.reload();
        this.messagerService.simple('Görevler iptal edildi');
      }else{
        this.messagerService.error('Hata',res.Error);
      }
    },
    (error:any)=>{
      this.messagerService.error('Hata',error.error.Error);
    });
  }
}
