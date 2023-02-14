import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
declare var $: any;
declare var w2ui: any;
declare var w2utils: any;
declare var w2popup: any;
declare var openPopup: any;
declare var openSelectedPopupGrid: any;
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css'],
})
export class UserComponent implements OnInit {
  apiUrl = environment.apiUrl;
  constructor() {}

  ngOnInit(): void {
    this.initializeScript();
  }
  initializeScript() {
    if (w2ui.UserLayout) {
      w2ui.UserLayout.destroy();
    }
    $().w2layout({
      name: 'UserLayout',
      panels: [
        {
          layoutName: 'UserLayout',
          type: 'main',
          style: '',
        },
      ],
      pageId: [1],
    });
    $('#UserLayout').w2render('UserLayout');
    if (w2ui.UserGrid) {
      w2ui.UserGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'UserGrid',
      layout: 'UserLayout',
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
        get: this.apiUrl + 'Users/GetAllForUi',
        remove: this.apiUrl + 'Users/SoftDeleteForUi',
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
          text: 'Tipi',
          field: 'Type',
          size: '100px',
          options: { items: ['Admin','User'], openOnFocus: true },
        },
        {
          searchable: true,
          text: 'Kullanıcı Adı',
          field: 'UserName',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Adı',
          field: 'FirstName',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Soyadı',
          field: 'LastName',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Durum',
          field: 'Status',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Engellendi mi?',
          field: 'Banned',
          size: '100px',
        },
        {
          searchable: true,
          text: 'Engellenme Sebebi',
          field: 'BannedMsg',
          size: '100px',
        },
      ],
      menu: [
        {
          id: 1,
          text: 'Yetkilerini Düzenle',
          icon: 'fa fa-snowflake-o',
        },
        {
          id: 2,
          text: 'İşlem Yetkilerini Düzenle',
          icon: 'fa fa-asterisk',
        },
        {
          id: 3,
          text: 'İstasyonları Düzenle',
          icon: 'fa fa-superpowers',
        },
      ],
      onAdd: function () {
        openPopup('Kişi Ekle', 'AddUserForm');
      },
      onEdit: function (event: any) {
        var selection = w2ui.UserGrid.getSelection();
        if (selection.length == 0) {
          return;
        }
        openPopup('Kişi Güncelle', 'UpdateUserForm', selection[0], {});
      },
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          openSelectedPopupGrid(
            'Yetkilerini Seç',
            'AuthorityUnSelectedGrid',
            'AuthoritySelectedGrid',
            { UserId: event.recid }
          );
        } else if (event.type == 'menuClick' && event.menuItem.id == 2) {
          openSelectedPopupGrid(
            'İşlem Yetkilerini Seç',
            'OperationClaimUnSelectedGrid',
            'OperationClaimSelectedGrid',
            { UserId: event.recid }
          );
        } else if (event.type == 'menuClick' && event.menuItem.id == 3) {
          openSelectedPopupGrid(
            'İstasyonları Seç',
            'StationUnSelectedGrid',
            'StationSelectedGrid',
            { UserId: event.recid }
          );
        }
      },
    });
    w2ui.UserLayout.html('main', w2ui.UserGrid);
    if (w2ui.AddUserForm) {
      w2ui.AddUserForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'AddUserForm',
      autoSize: true,
      fields: [
        {
          type: 'list',
          field: 'TypeView',
          required:true,
          html: {
            label: 'Kullanıcı Tipi',
          },
          options: { items: ['Admin','User'], openOnFocus: true },
        },
        {
          type: 'text',
          field: 'UserName',
          required:true,
          disabled: false,
          html: {
            label: 'Kullanıcı Adı',
          },
        },
        {
          type: 'password',
          field: 'Password',
          required:true,
          disabled: false,
          html: {
            label: 'Şifre',
          },
        },
        {
          type: 'text',
          field: 'FirstName',
          disabled: false,
          html: {
            label: 'Adı',
          },
        },
        {
          type: 'text',
          field: 'LastName',
          disabled: false,
          html: {
            label: 'Soyadı',
          },
        },
        {
          type: 'toggle',
          field: 'Status',
          disabled: false,
          html: {
            label: 'Durum',
          },
        },
        {
          type: 'toggle',
          field: 'Banned',
          disabled: false,
          html: {
            label: 'Engellendi mi?',
          },
        },
        {
          type: 'textarea',
          field: 'BannedMsg',
          disabled: false,
          html: {
            label: 'Engellenme Sebebi',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'Users/GetForUi',
        save: this.apiUrl + 'Users/AddForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.UserGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Ekle',
          onClick(event: any) {
            w2ui.AddUserForm.save();
          },
        },
      },
    });
    if (w2ui.UpdateUserForm) {
      w2ui.UpdateUserForm.destroy();
    }
    $().w2form({
      width: '600px',
      height: '400px',
      name: 'UpdateUserForm',
      autoSize: true,
      fields: [
        {
          type: 'int',
          text: 'Id',
          field: 'Id',
          required:true,
          disabled: true,
        },
        {
          type: 'list',
          field: 'TypeView',
          required:true,
          html: {
            label: 'Kullanıcı Tipi',
          },
          options: { items: ['Admin','User'], openOnFocus: true },
        },
        {
          type: 'text',
          field: 'UserName',
          required:true,
          disabled: true,
          html: {
            label: 'Kullanıcı Adı',
          },
        },
        {
          type: 'password',
          field: 'Password',
          disabled: false,
          html: {
            label: 'Şifre',
          },
        },
        {
          type: 'text',
          field: 'FirstName',
          disabled: false,
          html: {
            label: 'Adı',
          },
        },
        {
          type: 'text',
          field: 'LastName',
          disabled: false,
          html: {
            label: 'Soyadı',
          },
        },
        {
          type: 'toggle',
          field: 'Status',
          disabled: false,
          html: {
            label: 'Durum',
          },
        },
        {
          type: 'toggle',
          field: 'Banned',
          disabled: false,
          html: {
            label: 'Engellendi mi?',
          },
        },
        {
          type: 'textarea',
          field: 'BannedMsg',
          disabled: false,
          html: {
            label: 'Engellenme Sebebi',
          },
        },
      ],
      url: {
        get: this.apiUrl + 'Users/GetForUi',
        save: this.apiUrl + 'Users/UpdateForUi',
      },
      onSave: function (event: any) {
        w2popup.close();
        w2ui.UserGrid.reload();
      },
      actions: {
        btnSave: {
          text: 'Güncelle',
          onClick(event: any) {
            w2ui.UpdateUserForm.save();
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
        get: this.apiUrl + 'OperationClaims/GetAllUnSelectedByUserIdUi',
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
        get: this.apiUrl + 'OperationClaims/GetAllSelectedByUserIdUi',
        save: this.apiUrl + 'OperationClaims/SaveSelectedByUserIdUi',
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
    if (w2ui.AuthorityUnSelectedGrid) {
      w2ui.AuthorityUnSelectedGrid.destroy();
    }
    $().w2grid({
      base: true,
      name: 'AuthorityUnSelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Authorities/GetAllUnSelectedByUserIdUi',
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
          gridName: 'AuthorityUnSelectedGrid',
          id: 1,
          text: 'Gönder',
          icon: 'fa fa-arrow-right',
        },
      ],
      limit: 100,
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('AuthoritySelectedGrid');
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('AuthoritySelectedGrid');
      },
    });
    if (w2ui.AuthoritySelectedGrid) {
      w2ui.AuthoritySelectedGrid.destroy();
    }
    $().w2grid({
      name: 'AuthoritySelectedGrid',
      panel: 'main',
      show: { header: true, footer: true, toolbar: true },
      method: 'GET',
      url: {
        get: this.apiUrl + 'Authorities/GetAllSelectedByUserIdUi',
        save: this.apiUrl + 'Authorities/SaveSelectedByUserIdUi',
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
          gridName: 'AuthoritySelectedGrid',
          id: 1,
          text: 'Geri Gönder',
          icon: 'fa fa-arrow-left',
        },
        {
          gridName: 'AuthoritySelectedGrid',
          id: 2,
          text: 'Kaydet',
          icon: 'fa fa-save',
        },
      ],
      onMenuClick: function (event: any) {
        if (event.type == 'menuClick' && event.menuItem.id == 1) {
          this.transferSelectedRecords('AuthorityUnSelectedGrid');
        } else if (event.type == 'menuClick' && event.menuItem.id == 2) {
          this.transferSave();
        }
      },
      onDblClick: function (event: any) {
        this.transferSelectedRecords('AuthorityUnSelectedGrid');
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
        get: this.apiUrl + 'Stations/GetAllUnSelectedByUserIdUi',
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
        get: this.apiUrl + 'Stations/GetAllSelectedByUserIdUi',
        save: this.apiUrl + 'Stations/SaveSelectedByUserIdUi',
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
