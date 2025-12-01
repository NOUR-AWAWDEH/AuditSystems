import { ref } from "vue";
import { getActionOwnersByAdminIdRepository } from "@/services/userManagement/actionOwner/getActionOwnersByAdminIdRepository";

export function useGetActionOwnersByAdminId() {
  const actionOwners = ref([]);
  const isLoading = ref(false);
  const error = ref(null);

  const getActionOwnersByAdminId = async () => {
    isLoading.value = true;
    error.value = null;

    try {
      const result = await getActionOwnersByAdminIdRepository.getActionOwnersByAdminId();

      if (result.success) {
        actionOwners.value = (result.result || []).map((owner) => ({
          ...owner,
          fullName: `${owner.firstName} ${owner.lastName}`,
        }));
      } else {
        const errorMsg = result.message || "Failed to fetch action-owners";
        error.value = errorMsg;
        console.error("Failed to fetch action-owners:", errorMsg);
        console.error("Full error result:", result);
      }
    } catch (err) {
      error.value = err.message || "An error occurred while fetching action-owners";
      console.error("Error fetching action-owners:", err);
    } finally {
      isLoading.value = false;
    }
  };

  const addActionOwner = (newActionOwner) => {
    actionOwners.value.push(newActionOwner);
  };

  const removeActionOwner = (actionOwnerId) => {
    actionOwners.value = actionOwners.value.filter(
      (actionOwner) => actionOwner.id !== actionOwnerId
    );
  };

  const updateActionOwner = (updatedActionOwner) => {
    const index = actionOwners.value.findIndex(
      (actionOwner) => actionOwner.id === updatedActionOwner.id
    );
    if (index !== -1) {
      actionOwners.value[index] = updatedActionOwner;
    }
  };

  return {
    actionOwners,
    isLoading,
    error,
    getActionOwnersByAdminId,
    addActionOwner,
    removeActionOwner,
    updateActionOwner,
  };
}
