<template>
  <div class="text-center">
    <div class="text-h4 q-mt-md text-primary">Order History</div>
    <q-avatar class="q-mt-md" size="xl" square>
      <img :src="`../img/cart.png`" />
    </q-avatar>
    <div class="status q-mt-md text-subtitle2 text-negative" text-color="red">
      {{ state.status }}
    </div>
  </div>
  <q-scroll-area style="height: 55vh">
    <q-card class="q-ma-md">
      <q-list separator>
        <q-item style="border: 0">
          <q-item-section class="col-3 text-h6"> ID</q-item-section>
          <q-item-section class="col-5 text-h6">Date Created</q-item-section>
          <q-item-section class="col-5 text-h6">Total</q-item-section>
        </q-item>
        <q-item
          clickable
          v-for="order in state.orders"
          :key="order.id"
          @click="selectOrder(order.id)"
        >
          <q-item-section>
            {{ order.id }}
          </q-item-section>
          <q-item-section>
            {{ formatDate(order.orderDate) }}
          </q-item-section>
          <q-item-section>
            {{ order.orderAmount }}
          </q-item-section>
        </q-item>
      </q-list>
    </q-card>
  </q-scroll-area>
  <q-dialog
    v-model="state.itemSelected"
    transition-show="rotate"
    transition-hide="rotate"
  >
    <q-card style="min-width: 75vw">
      <q-card-actions align="right">
        <q-btn flat label="X" color="primary" v-close-popup class="text-h5" />
      </q-card-actions>
      <q-card-section>
        <div class="text-h4 text-bold text-primary text-center">
          Order {{ state.selectedOrder.id }}
        </div>
      </q-card-section>
      <q-card-section class="p-b-xl text-center"> </q-card-section>
      <q-list separator>
        <q-item style="border: 0">
          <q-item-section
            class="q-pl-sm text-subtitle2 col-5 text-left text-primary"
          >
            Quantities
          </q-item-section>
          <q-item-section class="col-2 text-subtitle2 text-left text-primary"
            >Product
          </q-item-section>
          <q-item-section class="text-subtitle2 text-right q-pr-lg text-primary"
            >Price
          </q-item-section>
        </q-item>
        <q-item style="border: 0">
          <q-item-section class="q-pl-md text-subtitle2">
            S O B
          </q-item-section>
        </q-item>
        <q-item v-for="item in state.orderDetails" :key="item.orderId">
          <q-item-section class="q-pl-md col-2">
            {{ item.qtySold }} {{ item.qtyOrdered }} {{ item.qtyBackOrdered }}
          </q-item-section>
          <q-item-section class="q-pl-lg col-7 text-left">
            {{ item.description }}
          </q-item-section>
          <q-item-section class="q-pr-lg text-right">
            ${{ item.costPrice }}
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section class="col-5"> </q-item-section>
          <q-item-section class="col-1 text-bold"> Subtotal: </q-item-section>
          <q-item-section class="text-right q-pr-lg">
            ${{ state.selectedOrder.orderAmount }}
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section class="col-5"> </q-item-section>
          <q-item-section class="col-1 text-bold"> Tax(%13): </q-item-section>
          <q-item-section class="text-right q-pr-lg">
            ${{ state.selectedOrder.orderTax }}
          </q-item-section>
        </q-item>
        <q-item>
          <q-item-section class="col-5"> </q-item-section>
          <q-item-section class="col-1 text-bold text-primary">
            Total:
          </q-item-section>
          <q-item-section class="text-right q-pr-lg text-primary">
            ${{ state.selectedOrder.orderTotal }}
          </q-item-section>
        </q-item>
        <q-item> </q-item>
      </q-list>
    </q-card>
  </q-dialog>
</template>

<script>
import { reactive, onMounted } from "vue";
import { formatDate } from "../utils/formatutils";
import { fetcher } from "../utils/apiutil";

export default {
  setup() {
    onMounted(() => {
      fetchOrders();
    });

    let state = reactive({
      status: "",
      orders: [],
      selectedOrder: [],
      user: [],
      itemSelected: false,
      orderDetails: [],
    });

    const selectOrder = async (orderId) => {
      try {
        // q-item click sends us the id, so we need to find the object
        state.selectedOrder = state.orders.find(
          (order) => order.id === orderId
        );
        state.dialogStatus = "";
        const payload = await fetcher(`Order/${orderId}/${state.user.email}`);
        state.orderDetails = payload;
        state.itemSelected = true;
      } catch (err) {
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const fetchOrders = async () => {
      try {
        state.user = JSON.parse(sessionStorage.getItem("customer"));
        state.status = "Loading Order History...";
        const payload = await fetcher(`Order/${state.user.email}`);
        state.orders = payload;
        state.status = `Loaded ${state.orders.length} Past Orders.`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    return {
      state,
      formatDate,
      selectOrder,
    };
  },
};
</script>
