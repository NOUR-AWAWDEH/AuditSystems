<template>
  <div class="min-h-screen bg-gray-50 *:min-h-full p-5">
    <JobTitleView />
    <AddUserForm />
  </div>

</template>

<script setup>
import JobTitleView from "./jobTitle/JobTitleView.vue";
import AddUserForm from "./users/components/AddUserForm.vue";

</script>
