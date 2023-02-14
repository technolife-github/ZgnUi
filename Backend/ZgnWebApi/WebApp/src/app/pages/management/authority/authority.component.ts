import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { MessagerService } from '../../../services/messager.service';
declare var $: any;
declare var w2ui: any;
declare var w2utils: any;
declare var w2popup: any;
declare var openPopup: any;
declare var openSelectedPopupGrid: any;
@Component({
  selector: 'app-authority',
  templateUrl: './authority.component.html',
  styleUrls: ['./authority.component.css'],
})
export class AuthorityComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor(private messagerService:MessagerService) { }

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    var self = this;
    if (w2ui.AuthorityLayout) {
      w2ui.AuthorityLayout.destroy();
    }
    $().w2layout({
      name: 'AuthorityLayout',
      panels: [
        {
          layoutName: 'AuthorityLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#AuthorityLayout').w2render('AuthorityLayout');
    if (w2ui.AuthorityGrid) {
      w2ui.AuthorityGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'AuthorityGrid',
      layout: 'AuthorityLayout',
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
        get: this.apiUrl + 'authorities/GetAllForUi',
        remove: this.apiUrl + 'authorities/SoftDeleteForUi',
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
          text: 'Yetki Adı',
          field: 'Name',
          size: '100px',
        },
      ],
      toolbar: {
        items: [
          { id: 'editClaim', type: 'button', text: 'İşlem Yetkilerini Düzenle', icon: 'fa fa-star' },
          { id: 'editStation', type: 'button', text: 'İstasyonları Düzenle', icon: 'fa fa-superpowers' },
        ],
        onClick(event: any) {
          var selections = w2ui.AuthorityGrid.getSelection();
          if (selections.length != 1) {
            self.messagerService.info("Lütfen 1 adet kayıt seçiniz.");
            return;
          }
          if (event.target == 'editClaim') {
            openSelectedPopupGrid(
              'İşlem Yetkilerini Seç',
              'OperationClaimUnSelectedGrid',
              'OperationClaimSelectedGrid',
              { AuthorityId: selections[0] }
            );
          } else if (event.target == 'editStation') {
            openSelectedPopupGrid(
              'İstasyonları Seç',
              'StationUnSelectedGrid',
              'StationSelectedGrid',
              { AuthorityId: selections[0] }
            );
          }
        }
      },
      onAdd: function () {
        openPopup('Yetki Ekle', 'AddAuthorityForm');
      },
      onEdit: function (event: any) {
        var selection = w2ui.AuthorityGrid.getSelection();
        if (selection.length == 0) {
          return;
        }
        openPopup('Yetki Güncelle', 'UpdateAuthorityForm', selection[0], {});
      },
    });
    w2ui.AuthorityLayout.html('main', w2ui.AuthorityGrid);
    if (w2ui.AddAuthorityForm) {
      w2ui.AddAuthorityForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'AddAuthorityForm',
      autoSize: true,
      fields: [
        {
          type: 'text',
          field: 'Name',
          required: true,
          disabled: false,
          html: {
            label: 'Yetki Adı',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'authorities/GetForUi',
        save: this.apiUrl + 'authorities/AddForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.AuthorityGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Ekle',
          onClick(event: any) {
            w2ui.AddAuthorityForm.save();
          },
        },
      },
    });
    if (w2ui.UpdateAuthorityForm) {
      w2ui.UpdateAuthorityForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'UpdateAuthorityForm',
      autoSize: true,
      fields: [
        {
          type: 'text',
          formName: 'UpdateAuthorityForm',
          field: 'Id',
          required: true,
          disabled: true,
        },
        {
          type: 'text',
          field: 'Name',
          required: true,
          html: {
            label: 'Yetki Adı',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'authorities/GetForUi',
        save: this.apiUrl + 'authorities/UpdateForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.AuthorityGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Güncelle',
          onClick(event: any) {
            w2ui.UpdateAuthorityForm.save();
          },
        },
      },
    });
    if (w2ui.OperationClaimUnSelectedGrid) {
      w2ui.OperationClaimUnSelectedGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'OperationClaimUnSelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'OperationClaims/GetAllUnSelectedByAuthorityIdUi',
      },
      header: 'İçermeyen Listesi',
      autoLoad: false,
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
          text: 'İşlem Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Açıklama',
          field: 'Description',
          size: '100px',
        },
      ],
      fixedBody: true,
      dataType: null,
      httpHeaders: {},
      menu: [
        {
          gridName: 'OperationClaimUnSelectedGrid',
          id: 1,
          text: 'Gönder',
          icon: 'fa fa-arrow-right',
        },
      ],
      limit: 100,
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('OperationClaimSelectedGrid');
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('OperationClaimSelectedGrid');
      },
    });
    if (w2ui.OperationClaimSelectedGrid) {
      w2ui.OperationClaimSelectedGrid.destroy();
    }
    $().w2grid({
      name: 'OperationClaimSelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'OperationClaims/GetAllSelectedByAuthorityIdUi',
        save: this.apiUrl + 'OperationClaims/SaveSelectedByAuthorityIdUi',
      },
      header: 'İçeren Listesi',
      width: '100%',
      height: '400px',
      recid: 'Id',
      columns: [
        {
          searchable: 'int',
          text: 'Id',
          field: 'Id',
          size: '100px',
        },
        {
          searchable: true,
          text: 'İşlem Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Açıklama',
          field: 'Description',
          size: '100px',
        },
      ],
      fixedBody: true,
      dataType: null,
      httpHeaders: {},
      menu: [
        {
          gridName: 'OperationClaimSelectedGrid',
          id: 1,
          text: 'Geri Gönder',
          icon: 'fa fa-arrow-left',
        },
        {
          gridName: 'OperationClaimSelectedGrid',
          id: 2,
          text: 'Kaydet',
          icon: 'fa fa-save',
        },
      ],
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('OperationClaimUnSelectedGrid');
        } else if (event.type == 'menuClick' && event.menuItem.id == 2) {
          this.transferSave();
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('OperationClaimUnSelectedGrid');
      },
    });
    if (w2ui.StationUnSelectedGrid) {
      w2ui.StationUnSelectedGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'StationUnSelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Stations/GetAllUnSelectedByAuthorityIdUi',
      },
      header: 'İçermeyen Listesi',
      autoLoad: false,
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
          text: 'İşlem Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Açıklama',
          field: 'Description',
          size: '100px',
        },
      ],
      fixedBody: true,
      dataType: null,
      httpHeaders: {},
      menu: [
        {
          gridName: 'StationUnSelectedGrid',
          id: 1,
          text: 'Gönder',
          icon: 'fa fa-arrow-right',
        },
      ],
      limit: 100,
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('StationSelectedGrid');
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('StationSelectedGrid');
      },
    });
    if (w2ui.StationSelectedGrid) {
      w2ui.StationSelectedGrid.destroy();
    }
    $().w2grid({
      name: 'StationSelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Stations/GetAllSelectedByAuthorityIdUi',
        save: this.apiUrl + 'Stations/SaveSelectedByAuthorityIdUi',
      },
      header: 'İçeren Listesi',
      width: '100%',
      height: '400px',
      recid: 'Id',
      columns: [
        {
          searchable: 'int',
          text: 'Id',
          field: 'Id',
          size: '100px',
        },
        {
          searchable: true,
          text: 'İşlem Adı',
          field: 'Name',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Açıklama',
          field: 'Description',
          size: '100px',
        },
      ],
      fixedBody: true,
      dataType: null,
      httpHeaders: {},
      menu: [
        {
          gridName: 'StationSelectedGrid',
          id: 1,
          text: 'Geri Gönder',
          icon: 'fa fa-arrow-left',
        },
        {
          gridName: 'StationSelectedGrid',
          id: 2,
          text: 'Kaydet',
          icon: 'fa fa-save',
        },
      ],
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('StationUnSelectedGrid');
        } else if (event.type == 'menuClick' && event.menuItem.id == 2) {
          this.transferSave();
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('StationUnSelectedGrid');
      },
    });
  }
}
