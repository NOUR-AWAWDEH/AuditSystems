import { describe, it, expect, vi, beforeEach } from "vitest";
import { ref } from "vue";
import { useUserActions } from "../useUserActions";
import { useDeleteUserById } from "@/composables/userManagement/users/useDeleteUserById";
import { useNotification } from "@/composables/notification/useNotification";

// Mock dependencies
vi.mock("@/composables/userManagement/users/useDeleteUserById");
vi.mock("@/composables/notification/useNotification");

describe("useUserActions", () => {
  let users;
  let emit;
  let mockDeleteUser;
  let mockShowNotification;

  beforeEach(() => {
    users = ref([
      { id: 1, name: "User 1", FirstName: "John", LastName: "Doe" },
      { id: 2, name: "User 2", FirstName: "Jane", LastName: "Doe" },
    ]);
    emit = vi.fn();

    mockDeleteUser = vi.fn();
    useDeleteUserById.mockReturnValue({ deleteUser: mockDeleteUser });

    mockShowNotification = vi.fn();
    useNotification.mockReturnValue({ showNotification: mockShowNotification });
  });

  it("should initialize with default values", () => {
    const { showAddUserModal, showEditUserModal, userToEdit, showDeleteDialog } = useUserActions(
      users,
      emit
    );

    expect(showAddUserModal.value).toBe(false);
    expect(showEditUserModal.value).toBe(false);
    expect(userToEdit.value).toBe(null);
    expect(showDeleteDialog.value).toBe(false);
  });

  describe("Add User", () => {
    it("should open and close add/edit modal correctly", () => {
      const { handleAddUser, handleCloseAddUserModal, showAddUserModal } = useUserActions(
        users,
        emit
      );

      handleAddUser();
      expect(showAddUserModal.value).toBe(true);

      handleCloseAddUserModal();
      expect(showAddUserModal.value).toBe(false);
    });

    it("should handle user added correctly", async () => {
      const { handleUserAdded, showAddUserModal } = useUserActions(users, emit);
      const newUser = { id: 3, name: "New User" };

      // Set modal to open initially to verify closing
      showAddUserModal.value = true;

      await handleUserAdded(newUser);

      expect(users.value).toHaveLength(3);
      expect(users.value[2]).toEqual(newUser);
      expect(emit).toHaveBeenCalledWith("user-added", newUser);
      expect(showAddUserModal.value).toBe(false);
    });
  });

  describe("Edit User", () => {
    it("should open edit modal with user details", () => {
      const { handleEditUser, showEditUserModal, userToEdit } = useUserActions(users, emit);
      const user = users.value[0];

      handleEditUser(user);

      expect(userToEdit.value).toEqual(user);
      expect(showEditUserModal.value).toBe(true);
    });

    it("should close edit modal and clear selection", () => {
      const { handleCloseEditUserModal, showEditUserModal, userToEdit, handleEditUser } =
        useUserActions(users, emit);
      const user = users.value[0];

      handleEditUser(user); // Open first
      handleCloseEditUserModal();

      expect(showEditUserModal.value).toBe(false);
      expect(userToEdit.value).toBe(null);
    });

    it("should handle user update successfully", async () => {
      const { handleUserUpdated, showEditUserModal } = useUserActions(users, emit);
      const updatedUser = {
        ...users.value[0],
        name: "Updated Name",
        FirstName: "John",
        LastName: "Doe",
      }; // Keep structure consistent

      // Set modal open
      showEditUserModal.value = true;

      await handleUserUpdated(updatedUser);

      expect(users.value[0]).toEqual(expect.objectContaining({ name: "Updated Name" }));
      expect(emit).toHaveBeenCalledWith("user-updated", updatedUser);
      expect(mockShowNotification).toHaveBeenCalledWith(
        expect.stringContaining("successfully"),
        "success"
      );
      expect(showEditUserModal.value).toBe(false);
    });

    it("should handle update error", async () => {
      const { handleUserUpdated } = useUserActions(users, emit);

      const consoleSpy = vi.spyOn(console, "error").mockImplementation(() => {});

      // Pass null to trigger an error when accessing properties
      await handleUserUpdated(null);

      expect(consoleSpy).toHaveBeenCalled();
      expect(mockShowNotification).toHaveBeenCalledWith("Failed to update user.", "error");

      consoleSpy.mockRestore();
    });
  });

  describe("Delete User", () => {
    it("should open delete dialog with correct details", () => {
      const {
        handleDeleteUser,
        showDeleteDialog,
        deleteDialogTitle,
        deleteDialogMessage,
        itemToDelete,
      } = useUserActions(users, emit);
      const user = users.value[0];

      handleDeleteUser(user);

      expect(itemToDelete.value).toEqual(user);
      expect(deleteDialogTitle.value).toBe("Delete User");
      expect(deleteDialogMessage.value).toContain("John Doe");
      expect(showDeleteDialog.value).toBe(true);
    });

    it("should confirm delete successfully", async () => {
      const { confirmDelete, handleDeleteUser, showDeleteDialog, itemToDelete } = useUserActions(
        users,
        emit
      );
      const user = users.value[0];

      mockDeleteUser.mockResolvedValue({ success: true });

      handleDeleteUser(user);
      await confirmDelete();

      expect(mockDeleteUser).toHaveBeenCalledWith(user.id);
      expect(users.value).toHaveLength(1);
      expect(users.value.find((u) => u.id === user.id)).toBeUndefined();
      expect(mockShowNotification).toHaveBeenCalledWith(
        expect.stringContaining("successfully"),
        "success"
      );
      expect(emit).toHaveBeenCalledWith("user-deleted", user.id);

      expect(showDeleteDialog.value).toBe(false);
      expect(itemToDelete.value).toBe(null);
    });

    it("should handle failed delete", async () => {
      const { confirmDelete, handleDeleteUser } = useUserActions(users, emit);
      const user = users.value[0];

      mockDeleteUser.mockResolvedValue({ success: false, message: "Delete failed" });

      handleDeleteUser(user);
      await confirmDelete();

      expect(mockDeleteUser).toHaveBeenCalledWith(user.id);
      expect(users.value).toHaveLength(2); // Nothing removed
      expect(mockShowNotification).toHaveBeenCalledWith("Delete failed", "error");
    });

    it("should return early if no item to delete", async () => {
      const { confirmDelete } = useUserActions(users, emit);
      await confirmDelete();
      expect(mockDeleteUser).not.toHaveBeenCalled();
    });
  });
});
