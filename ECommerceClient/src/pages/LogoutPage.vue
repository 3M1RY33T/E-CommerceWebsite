<template>
  <div class="text-center">
    <div class="text-h4 q-mt-lg">Customer Now Logged Out</div>
  </div>
</template>

<script>
import { reactive, onMounted } from "vue";
import { useRouter } from "vue-router";

export default {
  setup() {
    const router = useRouter();
    onMounted(() => {
      logout();
    });
    const logout = () => {
      sessionStorage.clear();
      state.status = "You have been logged out.";
    };
    let state = reactive({
      status: "",
    });
    return { state };
  },
};
</script>
