<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-toolbar-title> Case Study Application </q-toolbar-title>
        <q-btn flat round dense icon="reorder" class="q-mr-xs">
          <q-menu>
            <q-list style="min-width: 100px">
              <q-item clickable v-close-popup to="/">
                <q-item-section>Home</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="brands">
                <q-item-section>Brands</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="maps">
                <q-item-section>Branches</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="orders">
                <q-item-section>Orders</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="register">
                <q-item-section>Register</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="login">
                <q-item-section>Login</q-item-section>
              </q-item>
              <q-item clickable v-close-popup to="logout">
                <q-item-section>Logout</q-item-section>
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </q-toolbar>
    </q-header>
    <q-page-container>
      <router-view />
    </q-page-container>
    <q-footer elevated class="bg-grey-8 text-white">
      <q-toolbar>
        <q-toolbar-title>
          <div class="text-subtitle2">
            best viewed on mobile device — Yigit Yildiz &copy;{{
              new Date().getFullYear()
            }}
          </div>
        </q-toolbar-title>
      </q-toolbar>
    </q-footer>
  </q-layout>
</template>

<script>
import { defineComponent } from "vue";
export default defineComponent({
  name: "MainLayout",
  setup() {
    return {};
  },
});
</script>
