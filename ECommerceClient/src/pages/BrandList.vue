<template>
  <q-page>
    <div class="text-center">
      <q-avatar class="q-mt-xl" size="100px" square>
        <img :src="`/img/logo.png`" />
      </q-avatar>

      <div class="text-h2 q-mt-lg">Brands</div>

      <!-- <q-btn
      class="q-mt-xl"
      color="white"
      text-color="black"
      label="Load Brands"
      @click="loadTables"
    /> -->

      <div class="status q-mt-md text-subtitle2 text-negative" text-color="red">
        {{ state.status }}
      </div>

      <p></p>
      <q-select
        class="q-mt-lg q-ml-lg"
        v-if="state.brands.length > 0"
        style="width: 50vw; margin-bottom: 4vh; background-color: #fff"
        :option-value="'id'"
        :option-label="'name'"
        model-value=""
        :options="state.brands"
        label="Select a Brand"
        v-model="state.selectedBrandId"
        map-options
        @update:model-value="getProducts()"
        emit-value
      />

      <div
        class="text-h6 text-bold text-center text-primary"
        v-if="state.products.length > 0"
      >
        {{ state.selectedBrand.name }} PRODUCTS
      </div>
      <q-scroll-area style="height: 55vh">
        <q-card class="q-ma-md">
          <q-list separator>
            <q-item
              clickable
              v-for="item in state.products"
              :key="item.id"
              @click="selectProduct(item.id)"
            >
              <q-item-section avatar>
                <q-avatar style="height: 125px; width: 90px" square>
                  <img :src="`/img/${item.graphicName}`" />
                </q-avatar>
              </q-item-section>

              <q-item-section class="text-left">
                {{ item.productName }}
              </q-item-section>
            </q-item>
          </q-list>
        </q-card>
      </q-scroll-area>

      />
    </div>
    <q-dialog
      v-model="state.itemSelected"
      transition-show="rotate"
      transition-hide="rotate"
    >
      <q-card>
        <q-card-actions align="right">
          <q-btn flat label="X" color="primary" v-close-popup class="text-h5" />
        </q-card-actions>
        <q-card-section class="text-center">
          <q-avatar style="height: 250px; width: 250px" square>
            <img :src="`/img/${state.selectedProduct.graphicName}`" />
          </q-avatar>
        </q-card-section>
        <q-card-section>
          <div class="text-subtitle2 text-center">
            {{ state.selectedProduct.productName }}
          </div>
        </q-card-section>
        <q-card-section class="text-center">
          <q-chip icon="bookmark" color="primary" text-color="white"
            >Details
            <q-tooltip
              transition-show="flip-right"
              transition-hide="flip-left"
              text-color="white"
            >
              <p><b>Details: </b>{{ state.selectedProduct.description }}</p>
              <p>
                <b>MSRP (Manufacturer's Suggested Retail Price): </b
                >{{ state.selectedProduct.msrp }}
              </p>
              <p><b>Price: </b>{{ state.selectedProduct.costPrice }}</p>
              <p><b>Qty Available: </b>{{ state.selectedProduct.qtyOnHand }}</p>
              <p>
                <b>Qty On Back Order: </b
                >{{ state.selectedProduct.qtyOnBackOrder }}
              </p>
            </q-tooltip>
          </q-chip>
        </q-card-section>
        <q-card-section>
          <div class="row">
            <q-input
              v-model.number="state.qty"
              type="number"
              filled
              placeholder="qty"
              hint="Qty"
              dense
              style="max-width: 12vw"
            />&nbsp;
            <q-btn
              color="primary"
              label="Add To Cart"
              :disable="state.qty < 0"
              @click="addToCart()"
              style="max-width: 25vw; margin-left: 3vw"
            />
            <q-btn
              color="primary"
              label="View Cart"
              :disable="state.qty < 0"
              @click="viewCart()"
              style="max-width: 25vw; margin-left: 3vw"
            />
          </div>
        </q-card-section>

        <q-card-section class="text-center text-positive">
          {{ state.dialogStatus }}
        </q-card-section>
      </q-card>
    </q-dialog>
  </q-page>
</template>

<script>
import { reactive, onMounted } from "vue";
import { fetcher } from "../utils/apiutil";
import { useRouter } from "vue-router";
export default {
  setup() {
    const router = useRouter();
    onMounted(() => {
      getBrands();
    });
    const getBrands = async () => {
      try {
        state.status = "resetting exercise tables ...";
        //state.status = await fetcher(`Data`);
        state.brands = await fetcher(`Brand`);
        state.status = "Brands Loaded";
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const getProducts = async () => {
      try {
        state.selectedBrand = state.brands.find(
          (brand) => brand.id === state.selectedBrandId
        );
        state.status = `finding products for brand ${state.selectedBrand}...`;
        state.products = await fetcher(`Product/${state.selectedBrand.id}`);
        state.status = `loaded ${state.products.length} products for ${state.selectedBrand.name}`;
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const selectProduct = async (productid) => {
      try {
        // q-item click sends us the id, so we need to find the object
        state.selectedProduct = state.products.find(
          (item) => item.id === productid
        );
        state.itemSelected = true;
        state.dialogStatus = "";
        if (sessionStorage.getItem("tray")) {
          state.tray = JSON.parse(sessionStorage.getItem("tray"));
        }
      } catch (err) {
        console.log(err);
        state.status = `Error has occured: ${err.message}`;
      }
    };

    const addToCart = () => {
      let index = -1;
      if (state.cart.length > 0) {
        index = state.cart.findIndex(
          // is item already on the tray
          (item) => item.id === state.selectedProduct.id
        );
      }
      if (state.qty > 0) {
        index === -1 // add
          ? state.cart.push({
              id: state.selectedProduct.id,
              qty: state.qty,
              item: state.selectedProduct,
            })
          : (state.cart[index] = {
              // replace
              id: state.selectedProduct.id,
              qty: state.qty,
              item: state.selectedProduct,
            });
        state.dialogStatus = `${state.qty} item(s) added`;
      } else {
        index === -1 ? null : state.cart.splice(index, 1); // remove
        state.dialogStatus = `item(s) removed`;
      }
      sessionStorage.setItem("cart", JSON.stringify(state.cart));
      state.qty = 0;
    };

    let state = reactive({
      status: "",
      brands: [],
      products: [],
      selectedBrand: {},
      selectedBrandId: "",
      selectedProduct: {},
      dialogStatus: "",
      itemSelected: false,
      qty: 0,
      cart: [],
    });

    const viewCart = () => {
      router.push("cart");
    };

    return {
      state,
      getProducts,
      selectProduct,
      addToCart,
      viewCart,
    };
  },
};
</script>
