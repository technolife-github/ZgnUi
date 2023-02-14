import { Router } from '@angular/router';
import { MessagerService } from 'src/app/services/messager.service';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var initializeOptionData: any;
declare var w2popup: any;
declare var openPopup: any;
declare var openSelectedPopupGrid: any;
@Component({
  selector: 'app-station',
  templateUrl: './station.component.html',
  styleUrls: ['./station.component.css'],
})
export class StationComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(
    private messagerService:MessagerService,
    private router:Router
    ) { }

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.StationLayout) {
      w2ui.StationLayout.destroy();
    }
    $().w2layout({
      name: 'StationLayout',
      panels: [
        {
          layoutName: 'StationLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#StationLayout').w2render('StationLayout');
    if (w2ui.StationGrid) {
      w2ui.StationGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'StationGrid',
      layout: 'StationLayout',
      panel: 'main',
      show: {
        header: true,
        footer: true,
        toolbar: true,
        toolbarAdd: true,
        toolbarEdit: true,
        toolbarDelete: true,
      },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Stations/GetAllForUi',
        remove: this.apiUrl + 'Stations/SoftDeleteForUi',
      },
      autoLoad: true,
      recid: 'Id',
      advanceOnEdit: true,
      toolbar: {
          items: [
            { id: 'designer', type: 'button', text: 'Tasarım Düzenle', icon: 'fa fa-paint-brush' }
          ],
          onClick(event:any) {
            if (event.target == 'designer') {
              var selection = w2ui.StationGrid.getSelection();
              if (selection.length != 1) {
                self.messagerService.error('Lütfen bir adet istasyon seçiniz.');
                return;
              }
              self.router.navigateByUrl(`station-designer/${selection[0]}`)
            }
          }
      },
      columns: [
        {
          searchable: 'int',
          text: 'Id',
          field: 'Id',
          size: '100px',
        },
        {
          searchable: true,
          text: 'İstasyon Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: 'enum',
          text: 'İstasyon Tipi',
          field: 'Type',
          size: '100px',
          options: { items: ['Yerleştirme Noktası', 'Alma Noktası', 'Bırakma Noktası', 'İade Alma Noktası', 'İade Teslim Noktası'], openOnFocus: true },
        },
      ],
      onAdd: function () {
        openPopup('İstasyon Ekle', 'AddStationForm');
        initializeOptionData('AddStationForm', 'GroupCodes', self.apiUrl + 'Sap/GetGroups', 'Path', 'Name');
        initializeOptionData('AddStationForm', 'Nodes', self.apiUrl + 'BlueBotics/GetNodes', 'Name', 'Name');
      },
      onEdit: function (event: any) {
        var selection = w2ui.StationGrid.getSelection();
        if (selection.length == 0) {
          return;
        }
        openPopup('İstasyon Güncelle', 'UpdateStationForm', selection[0], {});
        initializeOptionData('UpdateStationForm', 'GroupCodes', self.apiUrl + 'Sap/GetGroups', 'Path', 'Name');
        initializeOptionData('UpdateStationForm', 'Nodes', self.apiUrl + 'BlueBotics/GetNodes', 'Name', 'Name');
      },
    });
    w2ui.StationLayout.html('main', w2ui.StationGrid);
    if (w2ui.AddStationForm) {
      w2ui.AddStationForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'AddStationForm',
      autoSize: true,
      fields: [
        {
          type: 'text',
          field: 'Name',
          required: true,
          disabled: false,
          html: {
            label: 'İstasyon Adı',
          },
        },
        {
          type: 'list',
          field: 'TypeView',
          required: true,
          html: {
            label: 'İstasyon Tipi',
          },
          options: { items: ['Yerleştirme Noktası', 'Alma Noktası', 'Bırakma Noktası', 'İade Alma Noktası', 'İade Teslim Noktası'], openOnFocus: true },
        },
        {
          type: 'enum',
          field: 'GroupCodes',
          html: {
            label: 'Grup Seçiniz',
          },
          options: { items: [], openOnFocus: true },
        },
        {
          type: 'enum',
          field: 'Nodes',
          html: {
            label: 'Node Seçiniz',
          },
          options: { items: [], openOnFocus: true },
        },
      ],
      url: {
        get: this.apiUrl + 'Stations/GetForUi',
        save: this.apiUrl + 'Stations/AddForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.StationGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Ekle',
          onClick(event: any) {
            w2ui.AddStationForm.save();
          },
        },
      },
    });
    if (w2ui.UpdateStationForm) {
      w2ui.UpdateStationForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'UpdateStationForm',
      autoSize: true,
      fields: [
        {
          type: 'int',
          text: 'Id',
          field: 'Id',
          required: true,
          disabled: true,
        },
        {
          type: 'text',
          field: 'Name',
          required: true,
          disabled: false,
          html: {
            label: 'İstasyon Adı',
          },
        },
        {
          type: 'list',
          field: 'TypeView',
          required: true,
          html: {
            label: 'İstasyon Tipi',
          },
          options: { items: ['Yerleştirme Noktası', 'Alma Noktası', 'Bırakma Noktası', 'İade Alma Noktası', 'İade Teslim Noktası'], openOnFocus: true },
        },
        {
          type: 'enum',
          field: 'GroupCodes',
          html: {
            label: 'Grup Seçiniz',
          },
          options: { items: [], openOnFocus: true },
        },
        {
          type: 'enum',
          field: 'Nodes',
          html: {
            label: 'Node Seçiniz',
          },
          options: { items: [], openOnFocus: true },
        },
      ],
      url: {
        get: this.apiUrl + 'Stations/GetForUi',
        save: this.apiUrl + 'Stations/UpdateForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.StationGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Güncelle',
          onClick(event: any) {
            w2ui.UpdateStationForm.save();
          },
        },
      },
    });
  }
}
