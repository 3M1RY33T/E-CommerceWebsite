<template>
  <div class="text-center">
    <q-avatar class="q-mt-xl" size="50px" square>
      <img :src="`/img/logo.png`" />
    </q-avatar>
    <div class="text-h4 q-mt-md text-primary">Cart Contents</div>
    <q-avatar class="q-mt-md" size="xl" square>
      <img :src="`../img/cart.png`" />
    </q-avatar>
    <div class="text-h6 text-positive">{{ state.status }}</div>
  </div>
  <div
    v-if="state.cart.length > 0"
    style="width: 90vw; margin-left: 5vw; margin-top: 1vh"
  >
    <q-item style="border: 0">
      <q-item-section class="col-4 qml-sm text-h7 text-primary">
        NAME</q-item-section
      >
      <q-item-section class="col-3 qml-sm text-h7 text-primary">
        QUANTITY</q-item-section
      >
      <q-item-section class="col-2 qml-sm text-h7 text-primary">
        MSRP</q-item-section
      >
      <q-item-section class="col-4 qml-sm text-h7 text-primary">
        EXTENDED</q-item-section
      >
    </q-item>
    <q-scroll-area style="height: 45vh">
      <q-scroll class="fit">
        <q-card class="q-ma-md">
          <q-list separators>
            <q-item clickable v-for="cartItem in state.cart" :key="cartItem.id">
              <q-item-section class="col-5">
                {{ cartItem.item.productName }}
              </q-item-section>
              <q-item-section class="col-2">
                {{ cartItem.qty }}
              </q-item-section>
              <q-item-section class="col-2">
                {{ cartItem.item.msrp }}
              </q-item-section>
              <q-item-section class="text-right">
                ${{ cartItem.item.costPrice }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-7"> </q-item-section>
              <q-item-section class="col-2 text-bold">
                Subtotal:</q-item-section
              >
              <q-item-section class="text-right">
                ${{ state.subtotal }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-7"> </q-item-section>
              <q-item-section class="col-2 text-bold">
                Tax(%13):</q-item-section
              >
              <q-item-section class="text-right">
                ${{ (state.tax = (state.subtotal / 100) * 13).toFixed(2) }}
              </q-item-section>
            </q-item>
            <q-item>
              <q-item-section class="col-7"> </q-item-section>
              <q-item-section class="col-2 text-bold" style="color: blue">
                Total:</q-item-section
              >
              <q-item-section class="text-right" style="color: blue">
                ${{ (state.total = state.subtotal + state.tax) }}
              </q-item-section>
            </q-item>
          </q-list>
        </q-card>
        <q-btn
          class="q-ml-md"
          icon="shopping_cart_checkout"
          color="primary"
          text-color="white"
          @click="saveCart"
          >Checkout
        </q-btn>
        <q-btn
          class="q-ml-md"
          icon="loop"
          color="primary"
          text-color="white"
          @click="clearCart"
          >Clear Cart
        </q-btn>
      </q-scroll>
    </q-scroll-area>
    <q-card-section class="text-center q-pt-sm"> </q-card-section>
    <q-card-section class="text-center">
      <!-- <q-chip icon="bookmark" color="primary" text-color="white"
        >Nutrional Info
        <q-tooltip transition-show="flip-right" transition-hide="flip-left">
          <table style="border: solid; margin: 3vh">
            <tr>
              <td style="width: 16%; font-weight: bold">Protein:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.proTot }}
              </td>
              <td style="width: 16%; font-weight: bold">Calories:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.calTot }}
              </td>
              <td style="width: 16%; font-weight: bold">Carbs:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.carbTot }}
              </td>
            </tr>
            <tr>
              <td style="width: 16%; font-weight: bold">Fibre:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.fbrTot }}gr
              </td>
              <td style="width: 16%; font-weight: bold">Choles:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.cholTot }}mg
              </td>
              <td style="width: 16%; font-weight: bold">Salt:</td>
              <td style="width: 16%; text-align: right; padding-right: 3vw">
                {{ state.saltTot }}mg
              </td>
            </tr>
          </table>
        </q-tooltip>
      </q-chip> -->
    </q-card-section>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { poster } from "../utils/apiutil";

export default {
  setup() {
    onMounted(() => {
      loadCart();
    });
    let state = reactive({
      status: "",
      cart: [],
      subtotal: 0.0,
      total: 0.0,
      tax: 0.0,
    });

    const loadCart = () => {
      if (sessionStorage.getItem("cart")) {
        state.cart = JSON.parse(sessionStorage.getItem("cart"));
        state.cart.forEach((cartItem) => {
          state.subtotal += cartItem.item.costPrice * cartItem.qty;
        });
      } else {
        state.cart = [];
      }
    };

    const saveCart = async () => {
      let user = JSON.parse(sessionStorage.getItem("customer"));
      let tray = JSON.parse(sessionStorage.getItem("cart"));
      try {
        state.status = "sending cart info to server";
        let trayHelper = { email: user.email, selections: tray };
        let payload = await poster("order", trayHelper);
        if (payload.indexOf("not") > 0) {
          state.status = payload;
        } else {
          clearCart();
          state.status = payload;
        }
      } catch (err) {
        console.log(err);
        state.status = `Error add Cart: ${err}`;
      }
    };

    const clearCart = () => {
      sessionStorage.removeItem("cart");
      state.cart = [];
      state.status = "Cart cleared";
    };

    return {
      state,
      clearCart,
      saveCart,
    };
  },
};
</script>
