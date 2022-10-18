import { MessagerService } from './../../../services/messager.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var w2popup: any;
declare var openPopup: any;
declare var initializeOptionData: any;

@Component({
  selector: 'app-vehicle',
  templateUrl: './vehicle.component.html',
  styleUrls: ['./vehicle.component.css'],
})
export class VehicleComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(private messagerService:MessagerService) {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.VehicleLayout) {
      w2ui.VehicleLayout.destroy();
    }
    $().w2layout({
      name: 'VehicleLayout',
      panels: [
        {
          layoutName: 'VehicleLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#VehicleLayout').w2render('VehicleLayout');
    if (w2ui.VehicleGrid) {
      w2ui.VehicleGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'VehicleGrid',
      layout: 'VehicleLayout',
      panel: 'main',
      show: {
        header: true,
        footer: true,
        toolbar: true,
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'BlueBotics/GetAllVehiclesForUi',
      },
      autoLoad: true,
      recid: 'Id',
      advanceOnEdit: true,
      columns: [
        {
          searchable: true,
          text: 'Araç Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Yük varmı?',
          field: 'Isloaded',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Görev No',
          field: 'Missionid',
          size: '100px',
        },
        {
          searchable: true,
          text: 'İşlem Durumu',
          field: 'Operatingstate',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Bağlantı Durumu',
          field: 'State.Connection',
          viewMask: '{0}',
          size: '120px',
        },
        {
          searchable: 'datetime',
          text: 'Tarih',
          field: 'Timestamp',
          size: '120px',
        },
        {
          searchable: true,
          text: 'Batarya Durumu',
          field: 'State.BatteryInfo',
          viewMask: 'Batarya Durumu: {0}% - Voltaj: {1}V',
          size: '240px',
        },
      ],
      menu: [
        {
          id: 1,
          text: 'Cihazı Ekle',
          icon: 'fa fa-plus',
        },
        {
          id: 2,
          text: 'Cihazı Çıkar',
          icon: 'fa fa-ban',
        },
      ],
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          let vehicleName=w2ui.VehicleGrid.get(event.recid).Name;
          openPopup('Cihaz Ekle', 'InsertVehicleForm',event.recid,{Name:vehicleName});
          initializeOptionData('InsertVehicleForm','ChargerNode',self.apiUrl+'BlueBotics/GetAllChargerNodesByLoginUser','Name','Name');
        }
        else if (event.type == 'menuClick' && event.menuItem.id == 2) {
          let vehicleName=w2ui.VehicleGrid.get(event.recid).Name;
          openPopup('Cihaz Çıkar', 'ExtractVehicleForm',event.recid,{Name:vehicleName});
        }
      },
    });
    w2ui.VehicleLayout.html('main', w2ui.VehicleGrid);
    if (w2ui.InsertVehicleForm) {
      w2ui.InsertVehicleForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'InsertVehicleForm',
      autoSize: true,

      fields: [
        {
          type: 'text',
          field: 'Name',
          required: true,
          disabled: true,
          html: {
            label: 'Cihaz Adı',
          },
        },
        {
          type: 'list',
          field: 'ChargerNode',
          required: true,
          html: {
            label: 'Yerleştirme Noktası',
          },
          options: { items: [], openOnFocus: true },
        },
      ],
      method:'GET',
      url: {
        save: self.apiUrl + 'BlueBotics/InsertNode',
      },
      onSave: function (event: any) {
        self.messagerService.simple('Cihaz Eklendi');
        w2popup.close();
        w2ui.VehicleGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Ekle',
          onClick(event: any) {
            let rec=w2ui.InsertVehicleForm.record;
            w2ui.InsertVehicleForm.url.save=`${self.apiUrl}BlueBotics/InsertNode/${rec.Name}/${rec.ChargerNode.id}`;
            w2ui.InsertVehicleForm.save();
          },
        },
      },
    });
    if (w2ui.ExtractVehicleForm) {
        w2ui.ExtractVehicleForm.destroy();
      }
      $().w2form({
        width: '600px',
        height: '400px',
        name: 'ExtractVehicleForm',
        autoSize: true,

        fields: [
          {
            type: 'text',
            field: 'Name',
            required: true,
            disabled: true,
            html: {
              label: 'Cihaz Adı',
            },
          },
        ],
        method:'GET',
        url: {
          save: self.apiUrl + 'BlueBotics/ExtractNode',
        },
        onSave: function (event: any) {
          self.messagerService.simple('Cihaz çıkarıldı');
          w2popup.close();
          w2ui.VehicleGrid.reload();
        },
        actions: {
          btnSave: {
            text: 'Ekle',
            onClick(event: any) {
              let rec=w2ui.ExtractVehicleForm.record;
              w2ui.ExtractVehicleForm.url.save=`${self.apiUrl}BlueBotics/ExtractNode/${rec.Name}`;
              w2ui.ExtractVehicleForm.save();
            },
          },
        },
      });
  }
}
